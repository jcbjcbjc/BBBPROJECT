using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using System.Text;

public class Multiple_Controller : MonoBehaviour
{
    private static Multiple_Controller _instance;
    public static Multiple_Controller Instance { get { if (!_instance) { _instance = GameObject.FindObjectOfType(typeof(Multiple_Controller)) as Multiple_Controller; } return _instance; } }

    public int ID;

    public bool isEnterGame = false;
    public bool isStartmyselfGame = false;
    
    public string isset ="";
    public string isban="";
    public string ismove ="";
    public string isbrick ="";
    public string isbreak ="";
    public bool isgameover=false;
    public bool  ConveyPoint=false;
    string _data = "";

    public NavigationMultiple Navigation;
    public CustomVar _game_configuration;

    public static Queue<GameObject> retry_queue;
    public GameObject _Platforms;
    public GameObject _good_particles;

    public GameObject cam;

    public Transform _all_parent;

    public GameObject[] _Player;

    public bool[] is_Condition;

    public GameObject[] SetPoint;

    public Transform[][] BanPoint;

    public Transform[] BeginPoint;
    public Transform[] EndPoint;

    public bool isplayer_set;
    public bool isplayer_ban;

    public int MovementPoint;

    public bool isMove;
    public bool isBrick;
    public bool isBreak;

    public bool isPlayer;

    public int Ban_Point;

    public int Bet_Number;

    public int order;

    public int[] RandomPool;

    GameObject[][] map;

    IEnumerator _playersuccess;
    IEnumerator _playerfail;

    IEnumerator _sorry;

    public bool Is_Start_UI = false;

    public bool _is_gameover = false;

    public bool _is_game = false;

    public bool _is_game_pre = false;

    public bool _music = true;

    float _Bet_time = 2f;
    void _create_all_parent()
    {
        if (_all_parent != null)
        {
            Destroy(_all_parent.gameObject);
        }
        //----------------------------------------------
        _all_parent = new GameObject().transform;
        _all_parent.position = new Vector3(0, 0, 0);
        _all_parent.name = "All_Parent";
        //----------------------------------------------
    }
    GameObject generate_platform(int i,int j)
    {
        var parent = GameObject.Find("All_Parent").transform;
        
        var obj = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Platforms/" + "_Platform"), parent);
        obj.name = "_Platform";
        obj.GetComponent<_platform>().row = i;
        obj.GetComponent<_platform>().col = j;
        return obj;
    }
    void Create_Map()
    {
        map = new GameObject[8][];
        for (int i = 1; i <= 7; i++) {
            map[i] = new GameObject[13];
        }
        for (int i = 1; i <= 7; i++)
        {
            for (int j = 1; j <= 12 - System.Math.Abs(i - 4); j++)
            {
                map[i][j] = generate_platform(i,j);
                map[i][j].transform.position = new Vector3((float)(1.5f *j-3.75f+0.75*System.Math.Abs(4-i)), 0, 1.3f*(i-1));
            }
        }
    }
    void Init()
    {
        Debug.Log("init");
        _Player = new GameObject[2];

        Navigation = new NavigationMultiple();
        Navigation.Init();
        _game_configuration = GameObject.Find("Custom").GetComponent<CustomVar>();
        BanPoint = new Transform[2][];
        for (int i = 0; i < 2; i++) {
            BanPoint[i] = new Transform[3];
        }
        cam = GameObject.Find("MainCamera");
        retry_queue = new Queue<GameObject>();

        BeginPoint = new Transform[2];
        EndPoint = new Transform[2];

        BeginPoint[0] = map[4][12].transform;
        BeginPoint[1] = map[4][1].transform;
        EndPoint[0] = map[4][1].transform;
        EndPoint[1] = map[4][12].transform;
        BeginPoint[0].GetComponent<_platform>().status = MapCode.StartorEndPoint;
        BeginPoint[1].GetComponent<_platform>().status = MapCode.StartorEndPoint;

        SetPoint = new GameObject[2];
        for (int i = 0; i <= 1; i++) {
            SetPoint[i] = new GameObject();
        }

        SetPoint[0].transform.position = new Vector3(0, 0, 0);
        SetPoint[1].transform.position = new Vector3(0, 0, 0);

        is_Condition=new bool[2];

        is_Condition[0] = false;
        is_Condition[1] = false;

        Ban_Point = 3;

        isplayer_set = false;
        
        isplayer_ban = false;

        isMove = false;
        isBrick = false;
        isBreak = false;

        isPlayer = false;

        RandomPool = new int[20];
        for (int i = 1; i <= 4; i++) {
            RandomPool[i] = 1;
        }
        for (int i = 5; i <= 8; i++)
        {
            RandomPool[i] = 2;
        }
        for (int i = 9; i <= 12; i++)
        {
            RandomPool[i] = 3;
        }
        for (int i = 13; i <= 16; i++)
        {
            RandomPool[i] = 4;
        }
        for (int i = 17; i <= 18; i++)
        {
            RandomPool[i] = 5;
        }
        RandomPool[19] = 6;
    }
    void Create_Player() {
        
        _Player[0] = PlayerManager.OpenPlayer("MulPlayer");
        _Player[1] = PlayerManager.OpenPlayer("Enemy");
        //_Player[0].GetComponent<SpriteRenderer>().color = _game_configuration._color_list[2];
        //_Player[1].GetComponent<SpriteRenderer>().color = _game_configuration._color_list[3];
        if (ID == 1)
        {
            _Player[0].transform.position = new Vector3(14.25f, 0, 3.9f);
            _Player[1].transform.position = new Vector3(-2.25f, 0, 3.9f);
        }
        else {
            _Player[1].transform.position = new Vector3(14.25f, 0, 3.9f);
            _Player[0].transform.position = new Vector3(-2.25f, 0, 3.9f);
        }
    }
    void Camera_Change (){
        Debug.Log("ChangeCamera");
        cam.transform.position = new Vector3(6, 50, 3.9f);
        if (ID == 2) {
            cam.transform.Rotate(0, 0, 180);
        }
    }
    void SendSelectOrderMessenge() {
        Debug.Log("start");
        if (ID == 1) {
            _is_game_pre = true;
            var obj = Game.Instance.GetPanel("Multiple_ingame");
            obj.GetComponent<Multiple_UI_Control>().SetStartUI(true);

            order = Random.Range(1, 3);
            

            Debug.Log("֪ͨ�Է���ʼ");
            SendGameMessege(GameCode.StartPre, order.ToString());

            if (order == ID)
            {
                isplayer_set = true;
                SendGameMessege(GameCode.ConveyPoint, "S" + ";" + "1");
            }
        }
    }
    public void SendRoomMessege(int RoomCode, string data) {
        NetConn.Instance.Send(RequestCode.Room, RoomCode, data);
    }
    public void SendGameMessege(int GameCode,string data) {
        NetConn.Instance.Send(RequestCode.Game, GameCode, data);
    }
    public void Onquest(int GaCode,string data) {
        if (GaCode == GameCode.EnterGame)
        {
            Debug.Log("EnterGame");
            isEnterGame = true;
        }
        else if (GaCode == GameCode.StartPre)
        {
            Debug.Log("StartPre");
            order = int.Parse(data);
            _is_game_pre = true;

            Is_Start_UI = true;
            

            Debug.Log("�ѽ���startpre");
            if (order == ID)
            {
                isplayer_set = true;
                SendGameMessege(GameCode.ConveyPoint, "S" + ";" + "1");
            }
        }
        else if (GaCode == GameCode.ToBan)
        {
            isplayer_ban = true;
            SendGameMessege(GameCode.ConveyPoint, "B" + ";" + "2");
        }
        else if (GaCode == GameCode.ToSet)
        {
            isplayer_set = true;
            SendGameMessege(GameCode.ConveyPoint, "S" + ";" + "1");
        }
        else if (GaCode == GameCode.EndPre)
        {
            _is_game_pre = false;
            _is_game = true;
            isPlayer = true;
            isStartmyselfGame = true;
        }
        else if (GaCode == GameCode.Set)
        {
            isset = data;
            Debug.Log("Set�ɹ�");
        }
        else if (GaCode == GameCode.Ban)
        {
            Debug.Log("�з�������û��");
            
            isban = data;
        }
        else if (GaCode == GameCode.Move)
        {
            ismove = data;
        }
        else if (GaCode == GameCode.Brick)
        {
            isbrick = data;
        }
        else if (GaCode == GameCode.Break)
        {
            isbreak = data;
        }
        else if (GaCode == GameCode.Convert)
        {
            isPlayer = true;
            isStartmyselfGame=true;
        }
        else if (GaCode == GameCode.ConditionSuccess)
        {
            is_Condition[1] = true;
        }
        else if (GaCode == GameCode.Success)
        {
            _data = data;
            isgameover = true;
        }
        else if (GaCode == GameCode.ConveyPoint) {
            _data = data;
            ConveyPoint = true;
        }
    }
    private void Awake()
    {
        ID = Game.Instance.Room.RoomPositionID;
        _create_all_parent();
        Create_Map();
        Init();
        Create_Player();
        Camera_Change();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        SendRoomMessege(ActionCode.StartGameConfirm,"");
    }
    private void Update()
    {
        if (Is_Start_UI) {
            var obj = Game.Instance.GetPanel("Multiple_ingame");
            obj.GetComponent<Multiple_UI_Control>().SetStartUI(true);
            Is_Start_UI = false;
        }
        if (isEnterGame) {
            Debug.Log("��ʼ��ǩ");
            SendSelectOrderMessenge();
            isEnterGame = false;
        }
        if (isStartmyselfGame) {
            StartMyself();
            isStartmyselfGame = false;
        }
        if (isset!="") {
            var position = isset.Split(";");
            int row = int.Parse(position[0]);
            int col = int.Parse(position[1]);
            _Player[1].GetComponent<Enemy>().Set_Player_to(map[row][col]);
            isset = "";
        }
        if (isban!="") {
            Debug.Log("�з�����");
            var position = isban.Split(";");
            int row = int.Parse(position[0]);
            int col = int.Parse(position[1]);
            _Player[1].GetComponent<Enemy>().Ban_Player_to(map[row][col]);
            
            isban = "";
        }
        if (ismove!="") {
            var position = ismove.Split(";");
            int row = int.Parse(position[0]);
            int col = int.Parse(position[1]);
            _Player[1].GetComponent<Enemy>().Move_Player_to(map[row][col]);
            ismove = "";
        }
        if (isbrick!="") {
            var position = isbrick.Split(";");
            int row = int.Parse(position[0]);
            int col = int.Parse(position[1]);
            _Player[1].GetComponent<Enemy>().Brick_Player_to(map[row][col]);
            isbrick = "";
        }
        if (isbreak!="") {
            var position = isbreak.Split(";");
            int row = int.Parse(position[0]);
            int col = int.Parse(position[1]);
            _Player[1].GetComponent<Enemy>().Break_Player_to(map[row][col]);
            isbreak = "";
        }
        if (isgameover) {
            Gameover(_data);
            _data = "";
            isgameover = false;
        }
        if (ConveyPoint) {
            BasePanel panel;
            Game.Instance._panelDict.TryGetValue("Multiple_ingame", out panel);
            var mes = _data.Split(";");

            panel.GetComponent<Multiple_UI_Control>().SetNumber(mes[0], int.Parse(mes[1]));
            _data = "";
            ConveyPoint = false;
        }
    }
    public void Convert()
    {
        //ui��ʾ�ж��㲻����ͣ������
        //
        //�ƽ�ת��Ȩ
        SendGameMessege(GameCode.Convert, "");
        
        isPlayer = false;
        isBrick = false;
        isMove = false;
        isBreak = false;
    }
    public void StartMyself()
    {
        //ui
        int RandomPoint= Random.Range(1, 20);
        MovementPoint = RandomPool[RandomPoint];
        SendGameMessege(GameCode.ConveyPoint, MovementPoint.ToString()+";"+"1");
    }

    public void Stage_Move_Player()
    {
        if (isPlayer)
        {
            if (isBreak)
            {
                return;
            }
            if (isBrick)
            {
                isBrick = false;

                isMove = true;
                //ui��ʾ
                return;
            }
            if (isMove)
            {
                //uiȡ��
                isMove = false;
                return;
            }
            isMove = true;
            //ui��ʾȫ������
            //
            ///////////////
        }
    }
    public void Stage_Brick_Player()
    {
        if (isPlayer)
        {
            if (isBreak)
            {
                return;
            }
            if (isMove)
            {
                isMove = false;

                isBrick = true;
                //ui��ʾ
                return;
            }
            if (isBrick)
            {
                //uiȡ��
                isBrick= false;
                return;
            }
            isBrick = true;
            //ui��ʾȫ������
            //
            ///////////////
        }
    }
    public void Stage_Break_Player()
    {
        if (isPlayer)
        {
            if (isBreak)
            {
                MovementPoint -= 3;
                SendGameMessege(GameCode.ConveyPoint, Multiple_Controller.Instance.MovementPoint.ToString() + ";" + "1");
                isBreak = false;
                if (MovementPoint <= 0)
                {
                    Convert();
                }
                return;
            }
            if (MovementPoint >= 3)
            {
                isBrick = false;
                isMove = false;

                Bet_Number = Random.Range(0, 2);
                if (Bet_Number == 1)
                {
                    isBreak = true;
                    //ui��ʾ
                    _playersuccess = _success1();
                    StartCoroutine(_playersuccess);
                }
                else
                {
                    _playerfail = _fail1();
                    StartCoroutine(_playerfail);
                    if (_music)
                    {
                        _game_configuration._sounds[0].Play();
                    }

                    isBreak= false;

                    MovementPoint -= 3;
                    Multiple_Controller.Instance.SendGameMessege(GameCode.ConveyPoint, Multiple_Controller.Instance.MovementPoint.ToString() + ";" + "1");
                    if (MovementPoint <= 0)
                    {
                        Convert();
                        return;
                    }
                }
            }
            else
            {
                //UI��ʾ
            }
        }
    }
    public void _click_sound()
    {
        if (_music)
        {
            _game_configuration._sounds[2].Play();
        }
    }
    //----------------------------------------------
    public void _close_game()
    {
        Application.Quit();
    }
    //----------------------------------------------
    IEnumerator _success1()
    {
        float elapsedTime = 0;
        //----------------------------------------------
        while (elapsedTime < _Bet_time)
        {
            //----------------------------------------------
            BasePanel panel;
            Game.Instance._panelDict.TryGetValue("Multiple_ingame", out panel);
            panel.GetComponent<Multiple_UI_Control>().Success(true);
            elapsedTime += Time.deltaTime;
            //----------------------------------------------
            yield return new WaitForEndOfFrame();

            panel.GetComponent<Multiple_UI_Control>().Success(false);
        }
    }
    IEnumerator _fail1()
    {
        float elapsedTime = 0;
        //----------------------------------------------
        while (elapsedTime < _Bet_time)
        {
            //----------------------------------------------
            BasePanel panel;
            Game.Instance._panelDict.TryGetValue("Multiple_ingame", out panel);
            panel.GetComponent<Multiple_UI_Control>().failure(true);
            elapsedTime += Time.deltaTime;
            //----------------------------------------------
            yield return new WaitForEndOfFrame();

            panel.GetComponent<Multiple_UI_Control>().failure(false);
        }
    }

    //IEnumerator _sorrymuch()
    //{
    //    float elapsedTime = 0;
    //    float time = 0.5f;
    //    //----------------------------------------------
    //    while (elapsedTime < time)
    //    {
    //        //----------------------------------------------
    //        _UI_Control.Instance._UI_Little[4].SetActive(true);
    //        elapsedTime += Time.deltaTime;
    //        //----------------------------------------------
    //        yield return new WaitForEndOfFrame();

    //        _UI_Control.Instance._UI_Little[4].SetActive(false);

    //    }
    //}
    public void Gameover(string data) {
        _is_gameover = true;
        _is_game = false;

        cam.transform.position = new Vector3(21, -157, -881);
        GameObject.Find("_theme").GetComponent<AudioSource>().mute = true;
        GameOver.Setfrom(PatternCode.Multiple);
        Game.Instance.ShowPanel("GameOverPanel");

        Game.Instance.HidePanel("Multiple_ingame");

        var obj = Game.Instance.GetPanel("Multiple_ingame");
        obj.GetComponent<Multiple_UI_Control>().SetStartUI(false);

        Destroy(_Player[0]);
        Destroy(_Player[1]);

        GameOver.SetWinnername(data);

        for (int i = 0; i <= 7;i++) {
            for (int j = 0; j <= 12; j++) {
                Destroy(map[i][j]);
            }
        }
        Destroy(BeginPoint[0]);
        Destroy(EndPoint[0]);
        Destroy(EndPoint[1]);
        Destroy(BeginPoint[1]);
        Destroy(SetPoint[0]);
        Destroy(SetPoint[1]);
        var Control = GameObject.Find("MultipleControl");
        Destroy(Control);
    }
    public string CreatePlayerMessege(int ID,int row,int col,int Point) {
        StringBuilder sb = new StringBuilder();
        sb.Append(ID.ToString());
        sb.Append(";");
        sb.Append(row.ToString());
        sb.Append(";");
        sb.Append(col.ToString());
        sb.Append(";");
        sb.Append(col.ToString());
        return sb.ToString();
    }
}

