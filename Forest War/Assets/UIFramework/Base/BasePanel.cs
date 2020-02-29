using UnityEngine;
using System.Collections;

public class BasePanel : MonoBehaviour {

    protected UIManager uiManager;

    // 为了Panel自身能够调用UIManager中的方法，同时不需要每个Panel分别new一个UIManager的实例
    // 当UIManager创建Panel实例时，将Manager自身传递给该字段（即所有Panel都引用一个UIManager）
    public UIManager UiManager
    {
        set
        {
            uiManager = value;
        }
    }
    
    /// <summary>
    /// 游戏界面刚显示
    /// </summary>
    public virtual void OnEnter()
    {

    }


    /// <summary>
    /// 界面暂停
    /// </summary>
    public virtual void OnPause()
    {

    }


    /// <summary>
    /// 界面继续
    /// </summary>
    public virtual void OnResume()
    {

    }


    /// <summary>
    /// 界面不显示,退出这个界面，界面被关系
    /// </summary>
    public virtual void OnExit()
    {

    }

    protected void PlayClickSound()
    {
        GameFacade.Instance.PlayComSound(AudioManager.buttonClickSound);
    }

}
