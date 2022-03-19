/***
 * 
 *    Title: 
 *           ���⣺ ControlManager      
 *    Description: 
 *           ���ܣ� Open fixed GameController 
 *    Date: 2022
 *    Version: 0.1�汾
 *    Modify Recoder: 
 *    
 *   
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
public class ControlManager :MonoBehaviour
{
	/* singleton */
	private static ControlManager _Instance = null;
	/* singleton instance */
	public static ControlManager GetInstance()
	{
		if (_Instance == null)
		{
			_Instance = new GameObject("_GameControlManager").AddComponent<ControlManager>();
		}
		return _Instance;
	}

    private void Awake()
    {
		MessageCenter.AddMsgListener(ProConst.STARTGAME_MESSAGE, OnStartGameResponse);
	}

    public void Start()
    {
        
    }
    public void Update()
    {
		
    }
    public static void Open(string[] strs)
	{
		int ID = int.Parse(strs[1]);
		int GamePattern = int.Parse(strs[2]);
		if (GamePattern == PatternCode.Multiple)
		{
			Game.Instance.GameConfig.RoomPositionID = ID;

            //Game.Instance.GameConfig.MapData =

            //Game.Instance.GameConfig.PlayerData =


            ///////////////////////////////////////////////////////////////////////////////

            OpenControlConnet("Set_Controller");

			//UIManager.GetInstance().ShowUIForms(ProConst.MULTIPLE_UIFORM);

			UIManager.GetInstance().CloseUIForms(ProConst.ROOM_UIFORM);
		}
		else if (GamePattern == PatternCode.quartic)
		{
			Game.Instance.GameConfig.RoomPositionID = ID;

			

		}
		else if (GamePattern == PatternCode.hexgon)
		{

			Game.Instance.GameConfig.RoomPositionID = ID;

			//Game.Instance.GameConfig.MapData =

			//Game.Instance.GameConfig.PlayerData =
		}
		else if (GamePattern == PatternCode.Setting) {
			Game.Instance.GameConfig.RoomPositionID = ID;





		}
	}
	public static void OpenControlConnet(string name) {
		GameObject obj = GameObject.Find(name);
		if (obj == null)
		{
			var parent = GameObject.Find("_game_control").transform;
			if (parent.gameObject != null)
			{
				obj = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Control/" + name), parent);
				obj.name = name;
				return;
			}
		}
		return;
	}
	public static void OpenControlLocal(string name) {
		GameObject obj = GameObject.Find(name);
		if (obj == null)
		{
			var parent = GameObject.Find("_game_control").transform;
			if (parent.gameObject != null)
			{
				obj = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Control/" + name), parent);
				obj.name = name;
				return;
			}
		}
		return;
	}
	

	/*error */
	private void OnStartGameResponse(KeyValuesUpdate kv) {
		string[] strArray = kv.Values as string[];

		string error = strArray[0];

		Debug.Log("messengeType:StartGame"+strArray);
		if (kv.Values == null) {
			return;
		}
		if (error == "-1") {
			Debug.Log("��һ�δ׼��");
			Game.Instance.ShowMessage("��һ�δ׼��");
			return;
		}
		if (error == "-2") {
			Debug.Log("�����Ƿ���");
			Game.Instance.ShowMessage("�����Ƿ���");
			return;
		}
		Open(strArray);
	}
	
}
