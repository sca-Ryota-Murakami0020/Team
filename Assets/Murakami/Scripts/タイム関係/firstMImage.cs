using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class firstMImage : MonoBehaviour
{
    [SerializeField] private Sprite[] numberImage;
    private int firstMCount;
    [SerializeField] private Image image;
    private totalGameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<totalGameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        firstMCount = Mathf.FloorToInt(gm.TotalTime / 60);
        image.sprite = numberImage[firstMCount];
    }
}
