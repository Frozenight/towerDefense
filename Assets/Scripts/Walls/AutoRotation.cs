using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotation : MonoBehaviour
{
    [SerializeField] GameObject sideWall;
    [SerializeField] GameObject cornerWall;

    private float raycastDistance = 4f;
    private float raycastDiagnolDistance = 6f;
    private float raycastHeightOffset = 1.5f;

    private void Start()
    {
        Recalculate(false);
    }

    void CheckForHits(Vector3 center, Vector3 North, Vector3 South, Vector3 West, Vector3 East, Vector3 NorthEast, Vector3 NorthWest, Vector3 SouthEast, Vector3 SouthWest, bool recalculation)
    {
        RaycastHit hit;
        bool NorthHit = false, SouthHit = false, WestHit = false, EastHit = false, NorthEastHit = false, NorthWestHit = false, SouthEastHit = false, SouthWestHit = false;
        if (Physics.Raycast(center, North, out hit, raycastDistance))
        {
            if (hit.collider.gameObject.tag == "Wall")
            {
                NorthHit = true;
                if (!recalculation)
                    hit.collider.gameObject.GetComponent<AutoRotation>().Recalculate(true);
            }
        }
        if (Physics.Raycast(center, South, out hit, raycastDistance))
        {
            if (hit.collider.gameObject.tag == "Wall")
            {
                SouthHit = true;
                if (!recalculation)
                    hit.collider.gameObject.GetComponent<AutoRotation>().Recalculate(true);
            }
        }
        if (Physics.Raycast(center, West, out hit, raycastDistance))
        {
            if (hit.collider.gameObject.tag == "Wall")
            {
                WestHit = true;
                if (!recalculation)
                    hit.collider.gameObject.GetComponent<AutoRotation>().Recalculate(true);
            }
        }
        if (Physics.Raycast(center, East, out hit, raycastDistance))
        {
            if (hit.collider.gameObject.tag == "Wall")
            {
                EastHit = true;
                if (!recalculation)
                    hit.collider.gameObject.GetComponent<AutoRotation>().Recalculate(true);
            }
        }
        if (Physics.Raycast(center, NorthEast, out hit, raycastDiagnolDistance))
        {
            if (hit.collider.gameObject.tag == "Wall")
            {
                NorthEastHit = true;
            }
        }
        if (Physics.Raycast(center, NorthWest, out hit, raycastDiagnolDistance))
        {
            if (hit.collider.gameObject.tag == "Wall")
            {
                NorthWestHit = true;
            }
        }
        if (Physics.Raycast(center, SouthEast, out hit, raycastDiagnolDistance))
        {
            if (hit.collider.gameObject.tag == "Wall")
            {
                SouthEastHit = true;
            }
        }
        if (Physics.Raycast(center, SouthWest, out hit, raycastDiagnolDistance))
        {
            if (hit.collider.gameObject.tag == "Wall")
            {
                SouthWestHit = true;
            }
        }
        RuleManager(NorthHit, SouthHit, WestHit, EastHit, NorthEastHit, NorthWestHit, SouthEastHit, SouthWestHit, recalculation);
    }

    void RuleManager(bool NorthHit, bool SouthHit, bool WestHit, bool EastHit, bool NorthEastHit, bool NorthWestHit, bool SouthEastHit, bool SouthWestHit, bool recalculation)
    {
        int hitCounter = (NorthHit ? 1 : 0) + (SouthHit ? 1 : 0) + (WestHit ? 1 : 0) + (EastHit ? 1 : 0);
        if (hitCounter == 4)
        {
            sideWall.gameObject.SetActive(false);
            cornerWall.gameObject.SetActive(true);
            transform.eulerAngles = Vector3.zero;
            WallBuilder._instance.ShowWallSelection(this.gameObject);
            return;
        }
        if (hitCounter == 3)
        {
            DoubleCheck();
            return;
        }
        if (hitCounter == 2)
        {
            if ((SouthHit) && (EastHit))
            {
                if (SouthEastHit & SouthWestHit) 
                { 
                    DoubleCheck();
                    return;
                }
                sideWall.gameObject.SetActive(false);
                cornerWall.gameObject.SetActive(true);
                transform.eulerAngles = Vector3.zero;
                WallBuilder._instance.ShowWallSelection(this.gameObject);
                return;
            }
            if ((SouthHit) && (WestHit))
            {
                if (SouthWestHit && SouthEastHit)
                {
                    DoubleCheck();
                    return;
                }
                sideWall.gameObject.SetActive(false);
                cornerWall.gameObject.SetActive(true);
                transform.eulerAngles = Vector3.zero;
                WallBuilder._instance.ShowWallSelection(this.gameObject);
                return;
            }
            if ((NorthHit) && (WestHit))
            {
                if (NorthWestHit && NorthEastHit)
                {
                    DoubleCheck();
                    return;
                }
                sideWall.gameObject.SetActive(false);
                cornerWall.gameObject.SetActive(true);
                transform.eulerAngles = Vector3.zero;
                WallBuilder._instance.ShowWallSelection(this.gameObject);
                return;
            }
            if ((NorthHit) && (EastHit))
            {
                if (NorthEastHit && NorthWestHit)
                {
                    DoubleCheck();
                    return;
                }
                sideWall.gameObject.SetActive(false);
                cornerWall.gameObject.SetActive(true);
                transform.eulerAngles = Vector3.zero;
                WallBuilder._instance.ShowWallSelection(this.gameObject);
                return;
            }
            if ((NorthHit)&&(SouthHit))
            {
                transform.Rotate(new Vector3(0, 90, 0));
                DoubleCheck();
            }
            DoubleCheck();
        }
        else
        {
            if ((NorthHit) || (SouthHit))
            {
                transform.Rotate(new Vector3(0, 90, 0));
                WallBuilder._instance.ShowWallSelection(this.gameObject);
            }
            if (hitCounter == 1)
            {
                DoubleCheck();
            }
        }

    }

    void Recalculate(bool recalculate)
    {
        Vector3 center = transform.position + new Vector3(0, raycastHeightOffset, 0);
        Vector3 North = transform.TransformDirection(Vector3.forward);
        Vector3 South = transform.TransformDirection(Vector3.back);
        Vector3 West = transform.TransformDirection(Vector3.left);
        Vector3 East = transform.TransformDirection(Vector3.right);
        Vector3 NorthEast = transform.TransformDirection(Quaternion.Euler(0, 45, 0) * Vector3.forward);
        Vector3 NorthWest = transform.TransformDirection(Quaternion.Euler(0, -45, 0) * Vector3.forward);
        Vector3 SouthEast = transform.TransformDirection(Quaternion.Euler(0, 45, 0) * Vector3.back);
        Vector3 SouthWest = transform.TransformDirection(Quaternion.Euler(0, -45, 0) * Vector3.back);

        if (!cornerWall.activeSelf)
            CheckForHits(center, North, South, West, East, NorthEast, NorthWest, SouthEast, SouthWest, recalculate);
    }

    void DoubleCheck()
    {
        Vector3 center = transform.position + new Vector3(0, raycastHeightOffset, 0);
        Vector3 North = transform.TransformDirection(Vector3.forward);
        Vector3 South = transform.TransformDirection(Vector3.back);
        Vector3 West = transform.TransformDirection(Vector3.left);
        Vector3 East = transform.TransformDirection(Vector3.right);
        RaycastHit hit;
        List<GameObject> otherWall = new List<GameObject>();

        if (Physics.Raycast(center, North, out hit, raycastDistance))
        {
            if (hit.collider.gameObject.tag == "Wall")
            {
                otherWall.Add(hit.collider.gameObject);
            }
        }
        if (Physics.Raycast(center, South, out hit, raycastDistance))
        {
            if (hit.collider.gameObject.tag == "Wall")
            {
                otherWall.Add(hit.collider.gameObject);
            }
        }
        if (Physics.Raycast(center, West, out hit, raycastDistance))
        {
            if (hit.collider.gameObject.tag == "Wall")
            {
                otherWall.Add(hit.collider.gameObject);
            }
        }
        if (Physics.Raycast(center, East, out hit, raycastDistance))
        {
            if (hit.collider.gameObject.tag == "Wall")
            {
                otherWall.Add(hit.collider.gameObject);
            }
        }

        foreach (GameObject myWall in otherWall)
        {
            if ((transform.gameObject.transform.rotation.eulerAngles != myWall.gameObject.transform.rotation.eulerAngles) && (!myWall.GetComponent<AutoRotation>().cornerWall.activeSelf))
            {
                myWall.GetComponent<AutoRotation>().ForceSwitch();
            }
        }

        WallBuilder._instance.ShowWallSelection(this.gameObject);
    }

    void ForceSwitch()
    {
        Vector3 center = transform.position + new Vector3(0, raycastHeightOffset, 0);
        Vector3 North = transform.TransformDirection(Vector3.forward);
        Vector3 South = transform.TransformDirection(Vector3.back);
        Vector3 West = transform.TransformDirection(Vector3.left);
        Vector3 East = transform.TransformDirection(Vector3.right);
        RaycastHit hit;
        int counter = 0;

        if (Physics.Raycast(center, North, out hit, raycastDistance))
        {
            if (hit.collider.gameObject.tag == "Wall")
            {
                counter++;
            }
        }
        if (Physics.Raycast(center, South, out hit, raycastDistance))
        {
            if (hit.collider.gameObject.tag == "Wall")
            {
                counter++;
            }
        }
        if (Physics.Raycast(center, West, out hit, raycastDistance))
        {
            if (hit.collider.gameObject.tag == "Wall")
            {
                counter++;
            }
        }
        if (Physics.Raycast(center, East, out hit, raycastDistance))
        {
            if (hit.collider.gameObject.tag == "Wall")
            {
                counter++;
            }
        }

        if (counter == 3)
        {
            sideWall.gameObject.SetActive(false);
            cornerWall.gameObject.SetActive(true);
            transform.eulerAngles = Vector3.zero;
        }

    }
}
