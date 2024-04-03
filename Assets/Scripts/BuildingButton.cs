using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingButton : MonoBehaviour
{
    [SerializeField] GameObject _buildingPrefab;
    [SerializeField] int buildingCost = 50;
    private Vector3 mousePosition;
    public float gridSize = 1f;

    private bool controlBoolean = false; //Bu degisken ile yeterli para olup olmadigi denetleniyor.
    private static bool _isObjectSelected = false;
    private GameObject phantomBuilding;
    private SpriteRenderer phantomSpriteRenderer;

    public void BuyBuilding() //Satin alma kontrol fonksiyonu
    {
        if (GameManager.gold >= buildingCost && !_isObjectSelected)
        {   
            GameManager.gold -= buildingCost;
            controlBoolean = true;
            _isObjectSelected = true;
        }
    }
    private void Start()
    {
        if (_buildingPrefab != null)
        {
            phantomSpriteRenderer = _buildingPrefab.GetComponent<SpriteRenderer>();
        }
    }
    private void Update()
    {
        if (_buildingPrefab == null || !controlBoolean)
        {
            return;
        }

        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Kameradaki mouse pozisyonunu oyun dunyasindaki pozisyona ceviriyor.
        mousePosition.z = 0f;
        Vector3 snappedPosition = SnapToGrid(mousePosition); // grid bloklar n n ortas na snap ediyor.

        if (!EventSystem.current.IsPointerOverGameObject()) //Mouse Cursor'i UI'in uzerinde degilken
        {
            if (phantomBuilding == null) // Eger daha once phantomBuilding uretilmemisse buraya giriyor.
            {
                phantomBuilding = Instantiate(_buildingPrefab, snappedPosition, Quaternion.identity); //prefab'ten phantomBuilding isimli instance uretiliyor.
                phantomBuilding.SetActive(true);
                phantomBuilding.GetComponent<BoxCollider2D>().enabled = false;

                SpriteRenderer _phantomSprite = phantomBuilding.GetComponent<SpriteRenderer>();
                _phantomSprite.color = new Color(_phantomSprite.color.r, _phantomSprite.color.g, _phantomSprite.color.b, 0.5f);//Alpha degeri atan yor.

            }
            else if (phantomBuilding != null)
            {
                SpriteRenderer _phantomSprite = phantomBuilding.GetComponent<SpriteRenderer>();
                phantomBuilding.transform.position = snappedPosition; //mouse cursor'u takip etmesi saglaniyor.
                Collider2D[] colliders = Physics2D.OverlapBoxAll(snappedPosition, phantomSpriteRenderer.bounds.size, 0);
                foreach (Collider2D collider in colliders)
                {
                    if (collider.gameObject != phantomBuilding)
                    {
                        // Change the color to red if there's an object at the intended position
                        _phantomSprite.color = Color.red;
                        return;
                    }
                    else
                    {
                        _phantomSprite.color = new Color(phantomSpriteRenderer.color.r, phantomSpriteRenderer.color.g, phantomSpriteRenderer.color.b, 0.5f);
                    }
                }

                // If no objects found, revert back to the semi-transparent color
                _phantomSprite.color = new Color(phantomSpriteRenderer.color.r, phantomSpriteRenderer.color.g, phantomSpriteRenderer.color.b, 0.5f);


            }

            if (Input.GetMouseButtonDown(0)) // Mouse Sol Tik
            {
                // Check if there's any object at the intended position
                Collider2D[] colliders = Physics2D.OverlapBoxAll(snappedPosition, phantomSpriteRenderer.bounds.size, 0);
                foreach (Collider2D collider in colliders)
                {
                    if (collider.gameObject != phantomBuilding)
                    {
                        return; // If there's an object at the intended position, don't place the building
                    }
                }
                Destroy(phantomBuilding); //cursor olarak kullanilan obje siliniyor.


                GameObject _toBuild = Instantiate(_buildingPrefab, snappedPosition, Quaternion.identity); // Gercek obje burada uretiliyor.
                _toBuild.SetActive(true);
                controlBoolean = false;
                _isObjectSelected = false;

                MonoBehaviour[] scriptComponents = _toBuild.GetComponents<MonoBehaviour>(); //_toBuild instance'imdaki scriptleri getiriyor.
                foreach (MonoBehaviour scriptComponent in scriptComponents)
                {
                    if (scriptComponent != null && scriptComponent != this)
                    {
                        scriptComponent.enabled = true; //baslangicta inactive olan scriptleri tek tek active hale getiriyor.
                    }
                }

            }
            else if (Input.GetKeyDown(KeyCode.Escape)) // ESC tusuna basilirsa cursorObjesi siliniyor ve para iadesi yapiliyor.
            {
                Destroy(phantomBuilding);
                controlBoolean = false;
                GameManager.gold += buildingCost;
                phantomBuilding = null;
                _isObjectSelected = false;
            }
        }


    }

    private Vector3 SnapToGrid(Vector3 position)
    {
        float x = Mathf.Floor(position.x / gridSize) * gridSize + gridSize/2;
        float y = Mathf.Floor(position.y / gridSize) * gridSize + gridSize/2;

        return new Vector3(x, y, 0f);
    }
}
