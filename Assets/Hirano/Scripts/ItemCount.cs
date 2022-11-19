using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ItemCount : MonoBehaviour
{
  private Text itemText=null;
    private float olditem=0;
    private GameManager gmanager;
    private player player;
    private int destroyedItemcount=0;
  

    void Start()
    {
        this.gmanager=FindObjectOfType<GameManager>();
        this.player=FindObjectOfType<player>();
        itemText=GetComponent<Text>();
        if(player!=null)
        {
            itemText.text="åÆÇÃêî:"+destroyedItemcount;
        }
    }

    // Update is called once per frame
    void Update()
    {
        destroyedItemcount=player.GSItemCount;

        if(olditem!=destroyedItemcount)
        {
            itemText.text="åÆÇÃêî:"+destroyedItemcount;
            olditem=destroyedItemcount;
        }
    }
}
