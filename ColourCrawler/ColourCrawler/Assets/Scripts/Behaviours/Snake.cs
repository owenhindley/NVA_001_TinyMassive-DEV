using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    public int stepSpeed = 1;
    public float changeDirectionProbability = 0.2f;
    public Vector2 startingPosition = Vector2.zero;
    private Vector2 actualPosition = Vector2.zero;

    public int lastX = 0;
    public int lastY = 0;

    public int snakeColorIndex;
    private Color32 color;

    public Vector2 direction = Vector2.right;

    void Start()
    {
        startingPosition = new Vector2(Mathf.FloorToInt(Random.Range(0, Main.WIDTH)), Mathf.FloorToInt(Random.Range(0, Main.HEIGHT)));
        color = Main.Instance.availableColors[snakeColorIndex];
        actualPosition = startingPosition;
        lastX = Mathf.FloorToInt(actualPosition.x);
        lastY = Mathf.FloorToInt(actualPosition.y);
        Main.Instance.SetPixel(lastX, lastY, color);
        direction = Vector2.left;
    }

    // Update is called once per frame
    void Update()
    {
        if (Main.Instance.IsStep){            
            if (Random.value < changeDirectionProbability){
                RandomizeDirection();
                return;
            }
            var newPos = (actualPosition + direction);
            if (newPos.x  <= 0.0f || newPos.x >= Main.WIDTH){
                RandomizeDirection();
                return;
            } else if (newPos.y <= 0.0f || newPos.y >= Main.HEIGHT){
                RandomizeDirection();
                return;
            } else {
                actualPosition = newPos;
            }

            var newX = Mathf.RoundToInt(actualPosition.x);
            var newY = Mathf.RoundToInt(actualPosition.y);

            if (newX != lastX || newY != lastY){
                             
                Main.Instance.SetPixel(lastX, lastY, color);
                lastX = newX;                
                lastY = newY;   
                Main.Instance.SetPixel(newX, newY, Color.white);
                transform.localPosition = new Vector3(lastX, lastY, 0.0f);
            }

            
            
        }
    }


    void RandomizeDirection(){
        // Debug.Log("Snake change direction from " + direction);
        
        if (direction == Vector2.up) {                     
            direction = Random.value > 0.5f ? Vector2.left : Vector2.right;
        }
        else if (direction == Vector2.down) {                     
            direction = Random.value > 0.5f ? Vector2.left : Vector2.right;
        }
        else if (direction == Vector2.left) {                     
            direction = Random.value > 0.5f ? Vector2.up : Vector2.down;
        }
        else if (direction == Vector2.right) {                     
            direction = Random.value > 0.5f ? Vector2.up : Vector2.down;
        }

        // Debug.Log("Snake change direction to " + direction);

    }
}
