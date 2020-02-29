using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class UIManager : BaseManager{

    /// [此工程不使用单例模式]
    /// 单例模式的核心
    /// 1，定义一个静态的对象 在外界访问 在内部构造
    /// 2，构造方法私有化

    //private static UIManager _instance;

    //public static UIManager Instance
    //{
    //    get
    //    {
    //        if (_instance == null)
    //        {
    //            _instance = new UIManager();
    //        }
    //        return _instance;
    //    }
    //}

    private Transform canvasTransform;
    private Transform CanvasTransform
    {
        get
        {
            if (canvasTransform == null)
            {
                canvasTransform = GameObject.Find("Canvas").transform;
            }
            return canvasTransform;
        }
    }
    private Dictionary<UIPanelType, string> panelPathDict;//存储所有面板Prefab的路径
    private Dictionary<UIPanelType, BasePanel> panelDict;//保存所有实例化面板的游戏物体身上的BasePanel组件
    private Stack<BasePanel> panelStack;
    private MessagePanel msgPanel;

    //初始化Manager.
    public UIManager(GameFacade facade) : base(facade)
    {
        ParseUIPanelTypeJson();
    }

    //public UIManager() { ParseUIPanelTypeJson(); }


    public override void OnInit()
    {
        base.OnInit();
        PushPanel(UIPanelType.Message);
        PushPanel(UIPanelType.Start);
        //
        
    }

    /// <summary>
    /// 把某个页面入栈，同时显示该页面
    /// </summary>
    public void PushPanel(UIPanelType panelType)
    {
        if (panelStack == null)
            panelStack = new Stack<BasePanel>();

        //判断一下栈里面是否有页面
        if (panelStack.Count > 0)
        {
            BasePanel topPanel = panelStack.Peek();
            topPanel.OnPause();
        }

        BasePanel panel = GetPanel(panelType);
        panel.OnEnter();
        //Debug.Log("push " + panel.name);
        panelStack.Push(panel);
    }

    /// <summary>
    /// 出栈，调用顶层页面的OnExit和下一个页面的OnResume.
    /// </summary>
    public void PopPanel()
    {
        if (panelStack == null)
            panelStack = new Stack<BasePanel>();

        if (panelStack.Count <= 0) return;

        //关闭栈顶页面的显示
        BasePanel topPanel = panelStack.Pop();
        //Debug.Log("pop " + topPanel.name);
        topPanel.OnExit();

        if (panelStack.Count <= 0) return;
        BasePanel topPanel2 = panelStack.Peek();
        topPanel2.OnResume();

    }

    /// <summary>
    /// 根据面板类型实例化并得到面板
    /// </summary>
    /// <returns></returns>
    private BasePanel GetPanel(UIPanelType panelType)
    {
        if (panelDict == null)
        {
            panelDict = new Dictionary<UIPanelType, BasePanel>();
        }

        //BasePanel panel;
        //panelDict.TryGetValue(panelType, out panel);//TODO

        BasePanel panel = panelDict.TryGet(panelType);

        if (panel == null)
        {
            //如果找不到，那么就找这个面板的prefab的路径，然后去根据prefab去实例化面板
            //string path;
            //panelPathDict.TryGetValue(panelType, out path);
            string path = panelPathDict.TryGet(panelType);
            GameObject instPanel = GameObject.Instantiate(Resources.Load(path)) as GameObject;
            instPanel.transform.SetParent(CanvasTransform,false);
            instPanel.GetComponent<BasePanel>().UiManager = this;  //传递自身引用给具体Panel，方便其调用方法.
            panelDict.Add(panelType, instPanel.GetComponent<BasePanel>());
            return instPanel.GetComponent<BasePanel>();
        }
        else
        {
            return panel;
        }

    }

    [Serializable]
    class UIPanelTypeJson
    {
        public List<UIPanelInfo> infoList;
    }
    private void ParseUIPanelTypeJson()
    {
        panelPathDict = new Dictionary<UIPanelType, string>();

        TextAsset ta = Resources.Load<TextAsset>("UIPanelType");

        UIPanelTypeJson jsonObject = JsonUtility.FromJson<UIPanelTypeJson>(ta.text);

        foreach (UIPanelInfo info in jsonObject.infoList) 
        {
            //Debug.Log(info.panelType);
            panelPathDict.Add(info.panelType, info.path);
        }
    }


    //↓以下均为新增代码

    public void InitMessagePanel(MessagePanel messagePanel)
    {
        this.msgPanel = messagePanel;
    }

    public void ShowMessage(string msg)
    {
        if(msgPanel == null)
        {
            Debug.Log("[ERROR]:MessagePanel null reference!");
        }
        msgPanel.ShowMessage(msg);
    }

    public void ShowMessageAsync(string msg)
    {
        if(msgPanel == null)
        {
            Debug.Log("[ERROR]:MessagePanel null reference!");
        }
        msgPanel.ShowMessageAsync(msg);
    }

    /// <summary>
    /// just for test
    /// </summary>
    //public void Test()
    //{
    //    string path ;
    //    panelPathDict.TryGetValue(UIPanelType.Knapsack,out path);
    //    Debug.Log(path);
    //}
}
