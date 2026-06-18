using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class Fill : MonoBehaviour
{
    public int value;
    [SerializeField] Text valueDisplay;
    [SerializeField] float speed;
    bool Combine;

    Image MyImage;

    public void FillValueUpdate(int valueIn)
    {
        value = valueIn; 
        valueDisplay.text = value.ToString();

        int colorIndex = getColorIndex(valueIn);
        Debug.Log(colorIndex + "Color Index");
        MyImage = GetComponent<Image>();
        MyImage.color = GameController.instance.fillCollers[colorIndex];
    }

    int getColorIndex(int valueInt)
    {
        int index = 0;
        while (valueInt !=1)
        {
            index++;
            valueInt /= 2;
        }

        index--;
        return index;
    }


    private void Update()
    {
        if (transform.localPosition != Vector3.zero)
        {
            Combine = false;
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, Vector3.zero, speed * Time.deltaTime);
            
        }
        else if (Combine == false)
        {
            if (transform.parent.GetChild(0) != this.transform)
            {
                Destroy(transform.parent.GetChild(0).gameObject);
            }
            Combine = true;
        }
    }

    public void Double()
    {
        value *= 2;
        GameController.instance.ScoreUpdate(value);
        valueDisplay.text = value.ToString();

        int colorIndex = getColorIndex(value);
        Debug.Log(colorIndex + "Color Index");
        MyImage.color = GameController.instance.fillCollers[colorIndex];

        GameController.instance.WinCheck(value);
    }
}
