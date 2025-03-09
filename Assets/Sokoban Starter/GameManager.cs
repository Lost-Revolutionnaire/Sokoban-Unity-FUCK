using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GameManager : MonoBehaviour
{

    
    private GameObject[,] map;
  
    bool checkCollisions(int posY, int posX, int v, int h,  List<GameObject> movingObjects, int numberOfObjects)
    {
        //Border collisions
        

        if(posY == map.GetLength(0)-2 && v == 1)
        {
            
            return false;
        }
        if (posY == 1 && v == -1)
        {
            return false;
        }
        if (posX == map.GetLength(1)-2 && h == 1)
        {
            return false;
        }
        if (posX == 1 && h == -1)
        {
            return false;
        }

        
        
        //Wall Block collisions
        if(map[posY + v, posX + h] != null)
        {
            if (map[posY + v, posX + h].layer == 9)
            {
                return false;
            }
        }


        //push to wall collisions

        if (map[posY, posX].layer == 6 || map[posY, posX].layer == 8 || map[posY, posX].layer == 10)
        {
            //ONLY for the player
            bool checkAround = true;
            int emergencyChecker = 0;
            int blocksAhead = 1;
            while(checkAround && emergencyChecker < 100) 
            {
                //vertical
                emergencyChecker++;
                if(emergencyChecker == 90)
                {
                    //Debug.Log("Emergency Checker thrown - error");
                }
                if(v != 0)
                {
                    if (map[posY+v*blocksAhead, posX] != null)
                    {
                        //There's a block in front of you!
                        //What kind??
                        if(map[posY + v * blocksAhead, posX].layer == 8 || map[posY + v * blocksAhead, posX]. layer == 10 || map[posY + v * blocksAhead, posX].layer == 6){
                            //It's either a sticky block or a smooth block-- there may be a chance you can push here. 
                            //But we gotta make sure it's not already touching a wall before we recurse...
                            if (posY+ v*blocksAhead == map.GetLength(0) - 2 && v == 1)
                            {

                                return false;
                            }
                            if (posY+v*blocksAhead == 1 && v == -1)
                            {
                                return false;
                            }
                            if (posX +h*blocksAhead== map.GetLength(1) - 1 && h == 1)
                            {
                                return false;
                            }
                            if (posX +h*blocksAhead== 1 && h == -1)
                            {
                                return false;
                            }
                            blocksAhead++;
                        }
                        else
                        {

                            //if it's a *moving* cliny, keep moving, otherwise stop.
                            if (map[posY + v * blocksAhead, posX].layer != 7 || map[posY, posX + v * blocksAhead].layer != 9)
                            {
                                return false;
                            }

                            bool movingClingy = false;
                            for (int w = 0; w < numberOfObjects; w++)
                            {
                                if (movingObjects[w].GetComponent<GridObject>().gridPosition.x == posX && movingObjects[w].GetComponent<GridObject>().gridPosition.y == posY + v * blocksAhead)
                                {
                                    //There's a moving block here that shares these coords already-- don't move this!!
                                    movingClingy = true;
                                }
                            }

                            if (movingClingy == false)
                            {
                                return false;
                            }
                            blocksAhead++;
                            //it's a wall or a clingy-- you've gotta stop right there!
                            
                        }
                    }
                    else
                    {
                        //Nothing is blocking you! Go ahead and move
                        checkAround = false;
                    }
                }
                else
                {
                    //you're not moving in this direction anyway!
                    checkAround = false;
                }
            }

            //Debug.Log("Are we getting here?");
            checkAround = true;
            emergencyChecker = 0;
            blocksAhead = 1;
            while (checkAround && emergencyChecker < 100)
            {
                //horizontal
                emergencyChecker++;
                if (emergencyChecker == 90)
                {
                    //Debug.Log("Emergency Checker thrown - error");
                }
                if (h != 0)
                {
                    if (map[posY, posX + h * blocksAhead] != null)
                    {
                        //There's a block in front of you!
                        //What kind??
                        if (map[posY, posX + h * blocksAhead].layer == 8 || map[posY, posX+ h *blocksAhead].layer == 10 || map[posY, posX + h * blocksAhead].layer == 6)
                        {
                            //It's either a sticky block or a smooth block-- there may be a chance you can push here. 
                            //But we gotta make sure it's not already touching a wall before we recurse...
                            if (posY + v * blocksAhead == map.GetLength(0) - 2 && v == 1)
                            {

                                return false;
                            }
                            if (posY + v * blocksAhead == 1 && v == -1)
                            {
                                return false;
                            }
                            if (posX + h * blocksAhead == map.GetLength(1) - 2 && h == 1)
                            {
                                return false;
                            }
                            if (posX + h * blocksAhead == 1 && h == -1)
                            {
                                return false;
                            }
                            blocksAhead++;
                        }
                        else
                        {
                            if (map[posY, posX + h * blocksAhead].layer != 7 || map[posY, posX + h * blocksAhead].layer != 9)
                            {
                                return false;
                            }

                            bool movingClingy = false;
                            for (int w = 0; w < numberOfObjects; w++)
                            {
                                if (movingObjects[w].GetComponent<GridObject>().gridPosition.x == posX + h * blocksAhead && movingObjects[w].GetComponent<GridObject>().gridPosition.y == posY)
                                {
                                    //There's a moving block here that shares these coords already-- don't move this!!
                                    movingClingy = true;
                                }
                            }

                            if (movingClingy == false)
                            {
                                return false;
                            }
                            blocksAhead++;
                        }
                    }
                    else
                    {
                        //Nothing is blocking you! Go ahead and move
                        checkAround = false;
                    }
                }
                else
                {
                    //you're not moving in this direction anyway!
                    checkAround = false;
                }
            }

        }


        return true;
    }

    bool checkStickyBabe(int i, int j, List<GameObject> movingObjects, int numberOfObjects)
    {
        if (map[i-1, j] != null)
        {
            for(int w = 0; w < numberOfObjects; w++)
            {
               if (movingObjects[w].GetComponent<GridObject>().gridPosition.x == j && movingObjects[w].GetComponent<GridObject>().gridPosition.y == i-1) 
               {
                    return true;
               }
            }
        }
        if (map[i + 1, j] != null)
        {
            for (int w = 0; w < numberOfObjects; w++)
            {
                if (movingObjects[w].GetComponent<GridObject>().gridPosition.x == j && movingObjects[w].GetComponent<GridObject>().gridPosition.y == i + 1)
                {
                    return true;
                }
            }
        }
        if (map[i, j-1] != null)
        {
            for (int w = 0; w < numberOfObjects; w++)
            {
                if (movingObjects[w].GetComponent<GridObject>().gridPosition.x == j-1 && movingObjects[w].GetComponent<GridObject>().gridPosition.y == i )
                {
                    //Debug.Log("There's a sticky on the side");
                    return true;
                    
                }
            }
        }
        if (map[i, j+1] != null)
        {
            for (int w = 0; w < numberOfObjects; w++)
            {
                if (movingObjects[w].GetComponent<GridObject>().gridPosition.x == j +1 && movingObjects[w].GetComponent<GridObject>().gridPosition.y == i )
                {
                    //Debug.Log("There's a sticky on the side");
                    return true;
                    
                }
            }
        }
        return false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created\
    void moveBlocks(int v, int h)
    {
        int numberOfObjects = 0;
        List<GameObject> movingObjects = new List<GameObject>();
        for (int stickyClause = 0; stickyClause < 20; stickyClause++)
        {
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 11; j++)
                {
                    //do logic to detect if they're moving this turn
                    if (map[i, j] != null)
                    {
                        //player
                        if (map[i, j].layer == 6 && stickyClause == 0)
                        {

                            if (checkCollisions(i, j, v, h, movingObjects, numberOfObjects))
                            {
                            movingObjects.Add(map[i, j]);
                            numberOfObjects++;
                            //map[i, j] = null;
                            }

                        }

                        //slick

                        if (map[i, j].layer == 8 && stickyClause % 2 == 0 )
                    {
                            //make it so that if it's already moved, don't move it again.
                            bool alreadyMoved = false;
                            for (int w = 0; w < numberOfObjects; w++)
                            {
                                if (movingObjects[w].GetComponent<GridObject>().gridPosition.x == j && movingObjects[w].GetComponent<GridObject>().gridPosition.y == i)
                                {
                                    //There's a moving block here that shares these coords already-- don't move this!!
                                    alreadyMoved = true;
                                }
                            }

                            bool playerAbsent = true;
                        int blocksBack = 1;
                        int emergencyCounter = 0;
                        while (playerAbsent && emergencyCounter < 100 && alreadyMoved == false)
                        {
                            emergencyCounter++;
                            if (emergencyCounter == 90)
                            {
                                //Debug.Log("Emergency Counter thrown - error");
                            }

                            if (map[i - v * blocksBack, j] != null)
                            {
                                if (map[i - v * blocksBack, j].layer == 6)
                                {
                                    //success! We are being pushed by a player. 
                                    playerAbsent = false;
                                    //Debug.Log("Am I running today???");
                                    if (checkCollisions(i, j, v, h, movingObjects, numberOfObjects))
                                    {
                                        movingObjects.Add(map[i, j]);
                                        numberOfObjects++;
                                        //map[i, j] = null;
                                    }
                                }
                                else if (v == 0)
                                {
                                    //oops! you're accidentally looking at yourself. that means you shouldn't be looking here.
                                    //Debug.Log("So, what, are we just running this???");
                                    playerAbsent = false;
                                }
                                else if (map[i - (v * blocksBack), j].layer == 8 || map[i - (v * blocksBack), j].layer == 10)
                                {
                                    //if it's a slick or sticky, then try again. 
                                    if(map[i - (v * blocksBack), j].layer == 10)
                                        {
                                            for (int w = 0; w < numberOfObjects; w++)
                                            {
                                                if (movingObjects[w].GetComponent<GridObject>().gridPosition.x == j && movingObjects[w].GetComponent<GridObject>().gridPosition.y == i - v * blocksBack)
                                                {
                                                    //Nevermind anything else! This sticky is moving and you should be pushed.
                                                    playerAbsent = false;
                                                    if (checkCollisions(i, j, v, h, movingObjects, numberOfObjects))
                                                    {
                                                        movingObjects.Add(map[i, j]);
                                                        numberOfObjects++;
                                                        //map[i, j] = null;
                                                    }
                                                }
                                            }
                                        }

                                    blocksBack++;
                                    //Debug.Log("So, what, are we just running this???");

                                }
                                else
                                {
                                    //you're next to a wall or a clingy, which cannot be pushed. End here as well.
                                    playerAbsent = false;
                                }
                            }
                            else
                            {
                                //There's nothing pushing on this side, maybe try something else
                                playerAbsent = false;
                            }
                        }
                        playerAbsent = true;
                        emergencyCounter = 0;
                        blocksBack = 1;

                        while (playerAbsent && emergencyCounter < 100 && alreadyMoved == false)
                        {
                            emergencyCounter++;
                            if (emergencyCounter == 90)
                            {
                                //Debug.Log("Emergency Counter thrown - error");
                            }

                            if (map[i, j - h * blocksBack] != null)
                            {
                                if (map[i, j - h * blocksBack].layer == 6)
                                {
                                    //success! We are being pushed by a player. 
                                    playerAbsent = false;
                                    //Debug.Log("Am I running today???");
                                    if (checkCollisions(i, j, v, h, movingObjects, numberOfObjects))
                                    {
                                        movingObjects.Add(map[i, j]);
                                        numberOfObjects++;
                                        //map[i, j] = null;
                                    }
                                }
                                else if (h == 0)
                                {
                                    //oops! you're accidentally looking at yourself. that means you shouldn't be looking here.
                                    //Debug.Log("So, what, are we just running this???");
                                    playerAbsent = false;
                                }
                                else if (map[i, j - h * blocksBack].layer == 8 || map[i, j - h * blocksBack].layer == 10)
                                {
                                        //if it's a slick or sticky, then try again. 

                                        if (map[i , j- h*blocksBack].layer == 10)
                                        {
                                            for (int w = 0; w < numberOfObjects; w++)
                                            {
                                                if (movingObjects[w].GetComponent<GridObject>().gridPosition.x == j - h*blocksBack && movingObjects[w].GetComponent<GridObject>().gridPosition.y == i)
                                                {
                                                    //Nevermind anything else! This sticky is moving and you should be pushed.
                                                    playerAbsent = false;
                                                    if (checkCollisions(i, j, v, h, movingObjects, numberOfObjects))
                                                    {
                                                        movingObjects.Add(map[i, j]);
                                                        numberOfObjects++;
                                                        //map[i, j] = null;
                                                    }
                                                }
                                            }
                                        }
                                        blocksBack++;
                                    //Debug.Log("So, what, are we just running this???");

                                }
                                else
                                {
                                    //you're next to a wall or a clingy, which cannot be pushed. End here as well.
                                    playerAbsent = false;
                                }
                            }
                            else
                            {
                                //There's nothing pushing on this side, maybe try something else
                                playerAbsent = false;
                            }
                        }


                    }

                        //sticky
                        if (map[i, j].layer == 10)
                        {
                            if(stickyClause % 2 ==1)
                            {
                                bool doneShit = false;
                                for (int w = 0; w < numberOfObjects; w++)
                                {
                                    if (movingObjects[w].GetComponent<GridObject>().gridPosition.x == j && movingObjects[w].GetComponent<GridObject>().gridPosition.y == i)
                                    {
                                        //There's a moving block here that shares these coords already-- don't move this!!
                                        doneShit = true;
                                    }
                                }


                                //Debug.Log("Am I doing anything at all");
                                if (checkStickyBabe(i, j, movingObjects, numberOfObjects) && doneShit == false)
                                {

                                    if (checkCollisions(i, j, v, h, movingObjects, numberOfObjects))
                                    {
                                        movingObjects.Add(map[i, j]);
                                        numberOfObjects++;
                                        //map[i, j] = null;
                                    }
                                }
                            }
                        }


                        //clingy
                        if (map[i, j].layer == 7 && stickyClause %2 == 0)
                        {
                            bool doneShit = false;
                            for (int w = 0; w < numberOfObjects; w++)
                            {
                                if (movingObjects[w].GetComponent<GridObject>().gridPosition.x == j && movingObjects[w].GetComponent<GridObject>().gridPosition.y == i)
                                {
                                    //There's a moving block here that shares these coords already-- don't move this!!
                                    doneShit = true;
                                }
                            }


                            //vertical
                            if (map[i + v, j] != null && doneShit == false)
                            { 
                                if(map[i + v, j].layer == 10 || map[i + v, j].layer == 6)
                                {
                                    //Debug.Log("There's a sticky above me");
                                    for (int w = 0; w < numberOfObjects; w++)
                                    {
                                        if (movingObjects[w].GetComponent<GridObject>().gridPosition.x == j && movingObjects[w].GetComponent<GridObject>().gridPosition.y == i + v )
                                        {
                                            //Nevermind anything else! This sticky is moving and you should be pushed.
                                            //playerAbsent = false;
                                            if (checkCollisions(i, j, v, h, movingObjects, numberOfObjects))
                                            {
                                                movingObjects.Add(map[i, j]);
                                                numberOfObjects++;
                                                //map[i, j] = null;
                                            }
                                        }
                                    }
                                }
                            }
                            //horizontal
                            if (map[i, j+h] != null && doneShit == false)
                            {
                                if (map[i, j+h].layer == 10 || map[i, j+h].layer == 6)
                                {
                                    //Debug.Log("There's a sticky above me");
                                    for (int w = 0; w < numberOfObjects; w++)
                                    {
                                        if (movingObjects[w].GetComponent<GridObject>().gridPosition.x == j+h && movingObjects[w].GetComponent<GridObject>().gridPosition.y == i)
                                        {
                                            //Nevermind anything else! This sticky is moving and you should be pushed.
                                            //playerAbsent = false;
                                            if (checkCollisions(i, j, v, h, movingObjects, numberOfObjects))
                                            {
                                                movingObjects.Add(map[i, j]);
                                                numberOfObjects++;
                                                //map[i, j] = null;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    
                }
            }
        }
        foreach (GameObject block in movingObjects)
        {
            map[block.GetComponent<GridObject>().gridPosition.y, block.GetComponent<GridObject>().gridPosition.x] = null;
        }
        foreach (GameObject block in movingObjects)
        {
            
            
           
            block.GetComponent<GridObject>().gridPosition.x += h;
            block.GetComponent<GridObject>().gridPosition.y += v;
            map[block.GetComponent<GridObject>().gridPosition.y, block.GetComponent<GridObject>().gridPosition.x] = block;
               
            
            
        }
    }

    void Start()
    {


        map = new GameObject[7, 12];



        GameObject[] blocks = GameObject.FindGameObjectsWithTag("Block");
        for(int i = 0; i < 6; i++)
        {
            for(int j = 0; j < 11; j++)
            {
               foreach(GameObject block in blocks)
                {
                   if(block.GetComponent<GridObject>().gridPosition.x == j && block.GetComponent<GridObject>().gridPosition.y == i)
                    {
                        //something good happens
                        //Debug.Log("Block detected at: "+i+ ", "+j+"");
                        map[i, j] = block;
                    }
                }
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.S))
        {
            moveBlocks(1, 0);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            moveBlocks(-1, 0);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            moveBlocks(0, -1);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            moveBlocks(0, 1);
        }
        
    }
}
