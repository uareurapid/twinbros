using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LevelCompletion : MonoBehaviour
{
    public Sprite[] levelImages;
    public GameObject imageToChange;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void setImage(int pos)
    {
        if(pos >= 0 && pos < levelImages.Length)
        {
            imageToChange.GetComponent<Image>().sprite = levelImages[pos];
        }
    }
}
