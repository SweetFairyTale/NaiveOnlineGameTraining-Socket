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
        
    }

    /// <summary>
    /// 新页面入栈(生命周期事件: A->B, A.OnPouse(), B.OnEnter())
    /// </summary>
    public BasePanel PushPanel(UIPanelType panelType)
    {
        if (panelStack == null)
            panelStack = new Stack<BasePanel>();

        if (panelStack.Count > 0)
        {
            BasePanel topPanel = panelStack.Peek();
            topPanel.OnPause();
        }

        BasePanel panel = GetPanel(panelType);
        panel.OnEnter();
        panelStack.Push(panel);
        return panel;
    }

    /// <summary>
    /// 当前页面出栈(生命周期事件: B->A, B.OnExit(), A.OnResume())
    /// 出栈，调用顶层页面的OnExit和下一个页面的OnResume.
    /// </summary>
    public void PopPanel()
    {
        if (panelStack == null)
            panelStack = new Stack<BasePanel>();

        if (panelStack.Count <= 0) return;

        BasePanel topPanel = panelStack.Pop();
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
        //panelDict.TryGetValue(panelType, out panel);

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
            panelPathDict.Add(info.panelType, info.path);
        }
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

    //提供在其他线程中PushPanel的方法.
    private UIPanelType panelTypeToPush = UIPanelType.None;   
    public void PushPanelAsync(UIPanelType panelType)
    {
        panelTypeToPush = panelType;
    }

    //提供在其他线程中PopPanel的方法.
    private bool isPopPanel = false;
    public void PopPanelAsync()
    {
        isPopPanel = true;
    }

    public override void Update()
    {
        if(panelTypeToPush != UIPanelType.None)  //监听到一个异步线程要push的PanelType.
        {
            PushPanel(panelTypeToPush);
            panelTypeToPush = UIPanelType.None;
        }

        if(isPopPanel)
        {
            PopPanel();
            isPopPanel = false;
        }
    }

    
}
