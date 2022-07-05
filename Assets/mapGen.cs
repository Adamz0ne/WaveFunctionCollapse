using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class mapGen : MonoBehaviour
{
    public Tilemap grid;
    public Tile spriteD;
    public Tile spriteU;
    public Tile spriteL;
    public Tile spriteR;
    public Tile spriteB;
    public Tile baseSprite;
    float timer;
    bool run;
    public class unit
    {
        bool aval;
        List<int> options = new List<int>();
        public unit()
        {
            aval = true;
            for (int i = 0; i < 5; i++)
                options.Add(i);
        }

        public int getValue(int input)
        {
            return options[input];
        }
        public void removeOp(int a)
        {
            options.Remove(a);
        }

        public int getSize()
        {
            return options.Count;
        }

        public bool getAval()
        {
            return aval;
        }

        public void setAval(bool input)
        {
            aval = input;
        }
    }
    public unit[] unitList = new unit[100];
    // Start is called before the first frame update
    Tile getSpriteById(int input)
    {
        switch(input)
        {
            case 0: return spriteU;
            case 1: return spriteR;
            case 2: return spriteD;
            case 3: return spriteL;
            case 4: return spriteB;
        }
        return null;
    }
    Vector3Int getGridPos(Vector3Int input)
    {
        return new Vector3Int(input.x - 5, input.y - 5, input.z);
    }

    Vector3Int getGridById(int input)
    {
        int x, y;
        x = input % 10;
        y = input / 10;
        return getGridPos(new Vector3Int(x,y));
    }

    void cellUpdate()
    {
        List<int> optionList = new List<int>();
        int minimum = 5;
        int random;
        int spriteId;
        for(int i = 0; i < 100; i++)
        {
            if (unitList[i].getSize() < minimum && unitList[i].getAval())
            {
                minimum = unitList[i].getSize();
            }
        }
        for(int i = 0; i < 100; i++)
        {
            if (unitList[i].getSize() == minimum && unitList[i].getAval())
                optionList.Add(i);
        }
        Debug.Log(optionList.Count);
        random = Random.Range(0, optionList.Count);
        random = optionList[random];
        unitList[random].setAval(false);
        spriteId = unitList[random].getValue(Random.Range(0, minimum));
        grid.SetTile(getGridById(random), getSpriteById(spriteId));
        removeOptions(random, spriteId);
    }

    void removeOptions(int cell, int id)
    {
        int x = cell % 10;
        int y = cell / 10;
        int left  = x - 1 + y * 10;
        int right = x + 1 + y * 10;
        int up    = x + (y + 1) * 10;
        int down  = x + (y - 1) * 10;
        if (x > 0)
        {
            switch(id)
            {
                case 0:
                    unitList[left].removeOp(3);
                    unitList[left].removeOp(4);
                    break;
                case 1:
                    unitList[left].removeOp(0);
                    unitList[left].removeOp(1);
                    unitList[left].removeOp(2);
                    break;
                case 2:
                    unitList[left].removeOp(3);
                    unitList[left].removeOp(4);
                    break;
                case 3:
                    unitList[left].removeOp(3);
                    unitList[left].removeOp(4);
                    break;
                case 4:
                    unitList[left].removeOp(0);
                    unitList[left].removeOp(2);
                    unitList[left].removeOp(1);
                    break;
            }
        }
        if(x < 9)
        {
            switch (id)
            {
                case 0:
                    unitList[right].removeOp(1);
                    unitList[right].removeOp(4);
                    break;
                case 1:
                    unitList[right].removeOp(1);
                    unitList[right].removeOp(4);
                    break;
                case 2:
                    unitList[right].removeOp(1);
                    unitList[right].removeOp(4);
                    break;
                case 3:
                    unitList[right].removeOp(0);
                    unitList[right].removeOp(2);
                    unitList[right].removeOp(3);
                    break;
                case 4:
                    unitList[right].removeOp(0);
                    unitList[right].removeOp(2);
                    unitList[right].removeOp(3);
                    break;
            }
        }
        if(y > 0)
        {
            switch (id)
            {
                case 0:
                    unitList[down].removeOp(1);
                    unitList[down].removeOp(3);
                    unitList[down].removeOp(0);
                    break;
                case 1:
                    unitList[down].removeOp(2);
                    unitList[down].removeOp(4);
                    break;
                case 2:
                    unitList[down].removeOp(2);
                    unitList[down].removeOp(4);
                    break;
                case 3:
                    unitList[down].removeOp(2);
                    unitList[down].removeOp(4);
                    break;
                case 4:
                    unitList[down].removeOp(0);
                    unitList[down].removeOp(1);
                    unitList[down].removeOp(3);
                    break;
            }
        }
        if(y < 9)
        {
            switch (id)
            {
                case 0:
                    unitList[up].removeOp(0);
                    unitList[up].removeOp(4);
                    break;
                case 1:
                    unitList[up].removeOp(0);
                    unitList[up].removeOp(4);
                    break;
                case 2:
                    unitList[up].removeOp(1);
                    unitList[up].removeOp(2);
                    unitList[up].removeOp(3);
                    break;
                case 3:
                    unitList[up].removeOp(0);
                    unitList[up].removeOp(4);
                    break;
                case 4:
                    unitList[up].removeOp(1);
                    unitList[up].removeOp(2);
                    unitList[up].removeOp(3);
                    break;
            }
        }
    }
    int iter;
    void Start()
    {
        for (int i = 0; i < 100; i++)
        {
            unitList[i] = new unit();
        }
        timer = 0;
        iter = 0;
        run = true;
    }

    string s;
    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(Input.GetKeyDown(KeyCode.Space))
        {
            run = !run;
        }
        if (timer >= 0.2 && iter < 100 && run)
        {
            cellUpdate();
            timer = 0;
            iter++;
        }
        if(iter == 100)
        {
            Debug.Log("Finished!");
            iter = 101;
        }
        if(Input.GetMouseButtonDown(0))
        {
            for(int i = 0; i < 100; i++)
            {
                grid.SetTile(getGridById(i), baseSprite);
                unitList[i] = new unit();
                iter = 0;
            }
        }
    }
}
