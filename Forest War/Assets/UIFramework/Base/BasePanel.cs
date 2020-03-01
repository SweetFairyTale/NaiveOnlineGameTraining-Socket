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
    /// 当前页面入栈，界面显示
    /// </summary>
    public virtual void OnEnter()
    {

    }


    /// <summary>
    /// 从当前面板跳转到其他面板，即新页面入栈，界面暂停
    /// </summary>
    public virtual void OnPause()
    {

    }


    /// <summary>
    /// 新页面出栈，当前界面恢复
    /// </summary>
    public virtual void OnResume()
    {

    }


    /// <summary>
    /// 当前面板被关闭，页面出栈
    /// </summary>
    public virtual void OnExit()
    {

    }

    protected void PlayClickSound()
    {
        GameFacade.Instance.PlayComSound(AudioManager.buttonClickSound);
    }

}
