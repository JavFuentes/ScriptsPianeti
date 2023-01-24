using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public static BoardManager sharedInstance;
    public List<Sprite> prefabs = new List<Sprite>();
    public GameObject currentPlanet;
    public int xSize, ySize;       

    private GameObject[,] planets;

    public bool isShifting { get; set; }

    private Planet selectedPlanet;
    public const int MinPlanetsToMatch = 2;

    //Audio
    public AudioClip matchSound;
    private AudioSource AudioBoardManager;    
    
    void Start()
    {   
        //El famoso singleton
        if (sharedInstance == null)
        {
            sharedInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }      
            Vector2 offset = currentPlanet.GetComponent<BoxCollider2D>().size;
            CreateInicialBoard(offset);              
            AudioBoardManager = GetComponent<AudioSource>();            
    }

    private void CreateInicialBoard(Vector2 offset)
    {
        planets = new GameObject[xSize, ySize];

        float startX = this.transform.position.x;
        float startY = this.transform.position.y;

        int idx = -1;

        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                GameObject newPlanet = Instantiate(currentPlanet,
                    new Vector3(startX + (offset.x * x),
                                startY + (offset.y * y),
                                0),
                    currentPlanet.transform.rotation
                    );

                newPlanet.name = string.Format("Planet[{0}][{1}]", x, y);

                do
                {
                    idx = Random.Range(0, prefabs.Count);
                }
                while ((x > 0 && idx == planets[x - 1, y].GetComponent<Planet>().id) ||
                    (y > 0 && idx == planets[x, y - 1].GetComponent<Planet>().id));

                Sprite sprite = prefabs[idx];
                newPlanet.GetComponent<SpriteRenderer>().sprite = sprite;
                newPlanet.GetComponent<Planet>().id = idx;

                newPlanet.transform.parent = this.transform;

                planets[x, y] = newPlanet;                 
            }
        }
    }

    public IEnumerator FindNullPlanets()
    {
        for (int x = 0; x < xSize; x++)
        {
            for (int y=0; y < ySize; y++)
            {
                if (planets[x, y].GetComponent<SpriteRenderer>().sprite == null)
                {
                    yield return StartCoroutine(MakePlanetsFall(x, y));
                    break;
                }
            }
        }

        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                planets[x, y].GetComponent<Planet>().FindAllMatches();
            }
        }        
     }

    private IEnumerator MakePlanetsFall(int x, int yStart, float shiftDelay = 0.05f)
    {
        isShifting = true;

        List<SpriteRenderer> renderers = new List<SpriteRenderer>();
        int nullPlanets = 0;

        for (int y = yStart; y < ySize; y++)
        {
            SpriteRenderer spriteRenderer = planets[x, y].GetComponent<SpriteRenderer>();
            if (spriteRenderer.sprite == null)
            {
                nullPlanets++;
            }
            renderers.Add(spriteRenderer);
        }

        for (int i = 0; i < nullPlanets; i++)
        {
            GUIManager.sharedInstance.Score += 10; 

            yield return new WaitForSeconds(shiftDelay);
            for (int j = 0; j < renderers.Count - 1;j++)
            {
                renderers[j].sprite = renderers[j + 1].sprite;
                renderers[j + 1].sprite = GetNewPlanets(x,ySize-1);
                //SFX al caer nuevos planetas
                AudioBoardManager.PlayOneShot(matchSound, 0.3f);
            }
        }

        isShifting = false;
    }

    private Sprite GetNewPlanets(int x, int y)
    {
        List<Sprite> possiblePlanets = new List<Sprite>();
        possiblePlanets.AddRange(prefabs);

        if (x > 0)
        {
            possiblePlanets.Remove(planets[x - 1, y].GetComponent<SpriteRenderer>().sprite);
        }

        if (x < xSize - 1)
        {
            possiblePlanets.Remove(planets[x + 1, y].GetComponent<SpriteRenderer>().sprite);
        }

        if (y > 0)
        {
            possiblePlanets.Remove(planets[x, y-1].GetComponent<SpriteRenderer>().sprite);
        }
        return possiblePlanets[Random.Range(0, possiblePlanets.Count)];
    }
}
