using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using EventCallbacks;
public class CustomLayoutGroup : MonoBehaviour
{

    private List<GameObject> group;
    public float spacing;
    public float speed = 1f;
    private BoxCollider2D box;

    private float jumpHeight;
    private float jumpRotate;
    [SerializeField]private float jumpSpeed;
    private Vector3[] points = new Vector3[3];


    // Start is called before the first frame update
    void Start()
    {

        box = GetComponent<BoxCollider2D>();
        group = new List<GameObject>();
        CreateGroup();
        ResetLocations();
        ResetGroup();
        print("here");
        print("there");

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Arc();
        }
    }

    public List<GameObject> GetAllChilds(GameObject Go)
    {
        List<GameObject> list = new List<GameObject>();
        for (int i = 0; i < Go.transform.childCount; i++)
        {
            if(Go.transform.GetChild(i).gameObject.activeSelf == false) {
                continue; 
            }
            list.Add(Go.transform.GetChild(i).gameObject);
        }
        return list;
    }

    private void CreateGroup()
    {
        group.Clear();
        group = GetAllChilds(this.gameObject);
        RestartEvent re = new RestartEvent();
        re.groupLenght = group.Count();
        EventSystem.Current.FireEvent(EVENT_TYPE.WORD_RESET, re);
    }

    public void ResetGroup()
    {

        int numberActive = group.Count;

        Debug.Log("Active Layout" + numberActive);

        if(numberActive == 0){ return; }

        bool first = true;
        bool left = true;
        int times = 1;
        int index = 0;

        List<(GameObject, Vector2, bool)> pos = new List<(GameObject, Vector2, bool)>();

        if(numberActive % 2 == 0) //even
        {
            foreach(GameObject g in group)
            {

                if(g.activeSelf == false) { continue; }

                g.name = index.ToString();
                index++;

                BoxCollider2D col = g.GetComponent<BoxCollider2D>();
                Vector3 size = col.bounds.size;

                float add;

                if (first)             
                    add = ((size.x / 2) * times) - ((spacing * times) / 2);
                else
                    add = ((size.x) * times) - size.x/2;


                if (left)
                {
                    pos.Add((g, new Vector2(transform.position.x + (add + (spacing * times)), transform.position.y), false));
                    left = false;
                }
                else
                {
                    pos.Add((g, new Vector2(transform.position.x - (add + (spacing * times)), transform.position.y), false));
                    left = true;
                    times++;

                    if (first)
                        first = false;
                }
            }
        }
        else //odd
        {

            Debug.Log("odd");
            foreach (GameObject g in group)
            {

                if (g.activeSelf == false) { continue; }

                g.name = index.ToString();
                index++;

                g.name = index.ToString();

                if (first)
                {
                    g.transform.position = transform.position;
                    first = false;
                    continue;
                }

                BoxCollider2D col = g.GetComponent<BoxCollider2D>();
                Vector3 size = col.bounds.size;

                float add;

                
                add = ((size.x) * times) - ((spacing * times) / 2);


                if (left)
                {
                    pos.Add((g, new Vector2(transform.position.x + (add + (spacing * times)), transform.position.y), false));
                    left = false;
                }
                else
                {
                    pos.Add((g, new Vector2(transform.position.x - (add + (spacing * times)), transform.position.y), false));
                    left = true;
                    times++;
                }

            }
        }

        //Sort Pos and give them name according to sorting
        Comparer cc = new Comparer();

        pos.Sort(cc);

        for (int i = 0; i < pos.Count(); i++)
        {
            pos[i].Item1.name = i.ToString();
        }



        GoOut(2f, true, pos);
    }

    public async void GoOut(float duration, bool backwards, List<(GameObject, Vector2, bool)> pos)
    {
        float end = Time.time + duration;
        float step = speed * Time.deltaTime;

        bool allAreAtPos = false;

        while (allAreAtPos == false)
        {
            for(int i = 0; i < pos.Count; i++)
            {
                (GameObject, Vector2, bool) tup = pos[i];
                tup.Item1.transform.position = Vector2.MoveTowards(tup.Item1.transform.position, tup.Item2, step);

                if (0.01f > Vector2.Distance(tup.Item1.transform.position, tup.Item2) && tup.Item3 == false)
                {
                    tup.Item3 = true;
                }
                pos[i] = tup;
            }

            allAreAtPos = pos.All(a => a.Item3 == true);
            await Task.Yield();
        }
    }

   

    public async void Arc()
    {
        float count = 0f;
        Vector3 target = setFallPoint(group[0].transform.position);
        setPoint(target, group[0]);
        jumpSpeed = 3f;
        int index = 0;
        while (index < group.Count)
        {
            count += jumpSpeed * Time.deltaTime;
            Vector3 m1 = Vector3.Lerp(points[0], points[1], count);
            Vector3 m2 = Vector3.Lerp(points[1], points[2], count);
            group[index].transform.position = Vector3.Lerp(m1, m2, count);
            group[index].transform.Rotate(0.0f, 0.0f, jumpRotate, Space.Self);
            jumpSpeed += 0.8f * Time.deltaTime;
            if (0.1f > Vector2.Distance(target, group[index].transform.position))
            {                               
                index++;
                if(index < group.Count)
                {
                    target = setFallPoint(group[index].transform.position);
                    setPoint(target, group[index]);
                    count = 0f;
                }
                
            }
            await Task.Yield();
        }
    }

    private void setPoint(Vector3 target, GameObject objectToMove)
    {
        points[0] = objectToMove.transform.position;
        //EndPoint
        points[2] = target;
        //ControllPoint, Increase y value too increase height of jump
        points[1] = points[0] + (points[2] - points[0]) / 2 + Vector3.up * jumpHeight;
    }

    private Vector3 setFallPoint(Vector3 pos)
    {
        float rand = Random.Range(-2,2);
        jumpHeight = Random.Range(6, 9);
        jumpRotate = (rand * -0.2f);
        return new Vector3(pos.x + rand, pos.y - 2.5f, pos.z); 
    }



    private void ResetLocations()
    {
        foreach(GameObject g in group)
        {
            g.transform.position = RandomPointInBounds(box.bounds);
        }
    }

    public Vector3 RandomPointInBounds(Bounds bounds)
    {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }

    class Comparer : IComparer<(GameObject, Vector2, bool)>
    {
        public int Compare((GameObject, Vector2, bool) x, (GameObject, Vector2, bool) y)
        {          
            // CompareTo() method
            return x.Item2.x.CompareTo(y.Item2.x);

        }
    }

}
