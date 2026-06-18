using System;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public Cell right;
    public Cell left;
    public Cell up;
    public Cell down;

    public Fill fill;

    private void OnEnable()
    {
        GameController.slide += Onslide;
    }



    private void OnDisable()
    {
        GameController.slide -= Onslide;
    }



    private void Onslide(string WhatWasSent)
    {
        cellCheck();
        Debug.Log(WhatWasSent);
        if (WhatWasSent == "w")
        {
            if (up != null)
                return;
            Cell currentCell = this;
            slideUp(currentCell);

        }
        if (WhatWasSent == "a")
        {
            if (left != null)
                return;
            Cell currentCell = this;
            slideLeft(currentCell);
        }
        if (WhatWasSent == "s")
        {
            if (down != null)
                return;
            Cell currentCell = this;
            slideDown(currentCell);
        }
        if (WhatWasSent == "d")
        {
            if (right != null)
                return;
            Cell currentCell = this;
            slideRight(currentCell);
        }

        GameController.ticker++;
        if (GameController.ticker ==4)
        {
            GameController.instance.SpawnFill();
        }
    }

    void slideUp(Cell currentCell)
    {
        if (currentCell.down == null)
        {
            return;
        }

        Debug.Log(currentCell.gameObject);
        if (currentCell.fill != null)
        {
            Cell nextCell = currentCell.down;
            while (nextCell.down != null && nextCell.fill == null)
            {
                nextCell = nextCell.down;
            }

            if (nextCell.fill != null)
            {
                if (currentCell.fill.value == nextCell.fill.value)
                {
                    nextCell.fill.Double();
                    nextCell.fill.transform.parent = currentCell.transform;
                    currentCell.fill = nextCell.fill;
                    nextCell.fill = null;
                }
                else if(currentCell.down.fill != nextCell.fill)
                {
                    Debug.Log("not Double");
                    nextCell.fill.transform.parent = currentCell.down.transform;
                    currentCell.down.fill = nextCell.fill;
                    nextCell.fill = null;
                }
            }
        }
        else
        {
            Cell nextCell = currentCell.down;
            while (nextCell.down != null && nextCell.fill == null)
            {
                Debug.Log("here");
                nextCell = nextCell.down;
            }
            if (nextCell.fill != null)
            {
                nextCell.fill.transform.parent = currentCell.transform;
                currentCell.fill = nextCell.fill;
                nextCell.fill = null;
                slideUp(currentCell);
                Debug.Log("Slide to empty");
            }
        }

        if (currentCell.down == null)
        {
            return;
        }
        slideUp(currentCell.down);
    }

    void slideLeft(Cell currentCell)
    {
        if (currentCell.right == null)
        {
            return;
        }

        Debug.Log(currentCell.gameObject);
        if (currentCell.fill != null)
        {
            Cell nextCell = currentCell.right;
            while (nextCell.right != null && nextCell.fill == null)
            {
                nextCell = nextCell.right;
            }

            if (nextCell.fill != null)
            {
                if (currentCell.fill.value == nextCell.fill.value)
                {
                    nextCell.fill.Double();
                    nextCell.fill.transform.parent = currentCell.transform;
                    currentCell.fill = nextCell.fill;
                    nextCell.fill = null;
                }
                else if (currentCell.right.fill != nextCell.fill)
                {
                    Debug.Log("not Double");
                    nextCell.fill.transform.parent = currentCell.right.transform;
                    currentCell.right.fill = nextCell.fill;
                    nextCell.fill = null;
                }
            }
        }
        else
        {
            Cell nextCell = currentCell.right;
            while (nextCell.right != null && nextCell.fill == null)
            {
                Debug.Log("here");
                nextCell = nextCell.right;
            }
            if (nextCell.fill != null)
            {
                nextCell.fill.transform.parent = currentCell.transform;
                currentCell.fill = nextCell.fill;
                nextCell.fill = null;
                slideLeft(currentCell);
                Debug.Log("Slide to empty");
            }
        }

        if (currentCell.right == null)
        {
            return;
        }
        slideLeft(currentCell.right);
    }

    void slideRight(Cell currentCell)
    {
        if (currentCell.left == null)
        {
            return;
        }

        Debug.Log(currentCell.gameObject);
        if (currentCell.fill != null)
        {
            Cell nextCell = currentCell.left;
            while (nextCell.left != null && nextCell.fill == null)
            {
                nextCell = nextCell.left;
            }

            if (nextCell.fill != null)
            {
                if (currentCell.fill.value == nextCell.fill.value)
                {
                    nextCell.fill.Double();
                    nextCell.fill.transform.parent = currentCell.transform;
                    currentCell.fill = nextCell.fill;
                    nextCell.fill = null;
                }
                else if (currentCell.left.fill != nextCell.fill)
                {
                    Debug.Log("not Double");
                    nextCell.fill.transform.parent = currentCell.left.transform;
                    currentCell.left.fill = nextCell.fill;
                    nextCell.fill = null;
                }
            }
        }
        else
        {
            Cell nextCell = currentCell.left;
            while (nextCell.left != null && nextCell.fill == null)
            {
                Debug.Log("here");
                nextCell = nextCell.left;
            }
            if (nextCell.fill != null)
            {
                nextCell.fill.transform.parent = currentCell.transform;
                currentCell.fill = nextCell.fill;
                nextCell.fill = null;
                slideRight(currentCell);
                Debug.Log("Slide to empty");
            }
        }

        if (currentCell.left == null)
        {
            return;
        }
        slideRight(currentCell.left);
    }


    void slideDown(Cell currentCell)
    {
        if (currentCell.up == null)
        {
            return;
        }

        Debug.Log(currentCell.gameObject);
        if (currentCell.fill != null)
        {
            Cell nextCell = currentCell.up;
            while (nextCell.up != null && nextCell.fill == null)
            {
                nextCell = nextCell.up;
            }

            if (nextCell.fill != null)
            {
                if (currentCell.fill.value == nextCell.fill.value)
                {
                    nextCell.fill.Double();
                    nextCell.fill.transform.parent = currentCell.transform;
                    currentCell.fill = nextCell.fill;
                    nextCell.fill = null;
                }
                else if (currentCell.up.fill != nextCell.fill)
                {
                    Debug.Log("not Double");
                    nextCell.fill.transform.parent = currentCell.up.transform;
                    currentCell.up.fill = nextCell.fill;
                    nextCell.fill = null;
                }
            }
        }
        else
        {
            Cell nextCell = currentCell.up;
            while (nextCell.up != null && nextCell.fill == null)
            {
                Debug.Log("here");
                nextCell = nextCell.up;
            }
            if (nextCell.fill != null)
            {
                nextCell.fill.transform.parent = currentCell.transform;
                currentCell.fill = nextCell.fill;
                nextCell.fill = null;
                slideDown(currentCell);
                Debug.Log("Slide to empty");
            }
        }

        if (currentCell.up == null)
        {
            return;
        }
        slideDown(currentCell.up);
    }

    void cellCheck()
    {
        if (fill == null)
        {
            return;
        }
        if (up != null)
        {
            if (up.fill == null)
                return;
            if (up.fill.value == fill.value)
                return;
        }
        if (down != null)
        {
            if (down.fill == null)
                return;
            if (down.fill.value == fill.value)
                return;
        }
        if (right != null)
        {
            if (right.fill == null)
                return;
            if (right.fill.value == fill.value)
                return;
        }
        if (left != null)
        {
            if (left.fill == null)
                return;
            if (left.fill.value == fill.value)
                return;
        }
        GameController.instance.GameOverCheck(); 
    }
}
