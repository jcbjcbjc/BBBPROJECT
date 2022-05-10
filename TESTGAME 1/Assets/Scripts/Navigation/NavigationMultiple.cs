using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
public class NavigationMultiple
{
    private MyPoint CurrentPoint;
    public bool isFind = false;
    //����С��ͼ
    public MyPoint[,] map ;
    //����̽������
    private Queue<MyPoint> ExploreList;
    public void Init()
    {
        map = new MyPoint[7, 12];
        for (int i = 0; i < 7; i++)
            for (int j = 0; j < 12; j++)
            {
                map[i, j] = new MyPoint
                {
                    row = i + 1,
                    col = j + 1,
                    status = 0
                };
                if (map[i, j].col > 12 - System.Math.Abs(map[i, j].row - 4))
                {
                    map[i, j].status = 1;
                }
            }
        ExploreList = new Queue<MyPoint>();
    }
    public bool isNavigation(MyPoint BeginPoint, MyPoint EndPoint)
    {
        map[EndPoint.row-1, EndPoint.col-1].status = 4;
        CurrentPoint = (MyPoint)BeginPoint;
        while (true)
        {
            if (4 - CurrentPoint.row > 0)
            {
                for (int i = CurrentPoint.row - 1; i <= System.Math.Min(CurrentPoint.row + 1, 7); i++)
                {
                    if (i <= 0) { continue; }
                    for (int j = CurrentPoint.col - 1; j <= System.Math.Min(CurrentPoint.col + 1, 12 - System.Math.Abs(i - 4)); j++)
                    {
                        if (isFind)
                        {
                            Clearmap();
                            return true;
                        }
                        if (j <= 0) { continue; }
                        if (i == CurrentPoint.row && j == CurrentPoint.col) { continue; }
                        if (i == CurrentPoint.row - 1 && j == CurrentPoint.col + 1) { continue; }
                        if (i == CurrentPoint.row + 1 && j == CurrentPoint.col - 1) { continue; }
                        switch (map[i - 1, j - 1].status)
                        {
                            //���û̽�������ʼ������ڵ����Ϣ
                            case 0:
                                map[i - 1, j - 1].status = 2;
                                ExploreList.Enqueue(map[i - 1, j - 1]);
                                break;
                            //̽�������������ڵ���Ϣ
                            case 1:
                                break;
                            case 2:
                                break;
                            case 4:
                                //�ҵ��յ�
                                isFind = true;
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            else if (4 - CurrentPoint.row < 0)
            {
                for (int i = CurrentPoint.row - 1; i <= System.Math.Min(CurrentPoint.row + 1, 7); i++)
                {
                    if (i <= 0) { continue; }
                    for (int j = CurrentPoint.col - 1; j <= System.Math.Min(CurrentPoint.col + 1, 12 - System.Math.Abs(i - 4)); j++)
                    {
                        if (isFind)
                        {
                            Clearmap();
                            return true;
                        }
                        if (j <= 0) { continue; }

                        if (i == CurrentPoint.row && j == CurrentPoint.col) { continue; }
                        if (i == CurrentPoint.row - 1 && j == CurrentPoint.col - 1) { continue; }
                        if (i == CurrentPoint.row + 1 && j == CurrentPoint.col + 1) { continue; }
                        switch (map[i - 1, j - 1].status)
                        {
                            //���û̽�������ʼ������ڵ����Ϣ
                            case 0:
                                map[i - 1, j - 1].status = 2;
                                ExploreList.Enqueue(map[i - 1, j - 1]);
                                break;
                            //̽�������������ڵ���Ϣ
                            case 1:
                                break;
                            case 2:
                                break;
                            case 4:
                                //�ҵ��յ�
                                isFind = true;
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            else {
                for (int i = CurrentPoint.row - 1;  i <= System.Math.Min(CurrentPoint.row + 1, 7); i++)
                {
                    if (i <= 0) { continue; }
                    for (int j = CurrentPoint.col - 1;  j <= System.Math.Min(CurrentPoint.col + 1, 12 - System.Math.Abs(i - 4)); j++)
                    {
                        if (isFind)
                        {
                            Clearmap();
                            return true;
                        }
                        if (j <= 0) { continue; }
                        if (j == CurrentPoint.col + 1 && i != CurrentPoint.row) { continue; }
                        switch (map[i - 1, j - 1].status)
                        {
                            //���û̽�������ʼ������ڵ����Ϣ
                            case 0:
                                map[i - 1, j - 1].status = 2;
                                ExploreList.Enqueue(map[i - 1, j - 1]);
                                break;
                            //̽�������������ڵ���Ϣ
                            case 1:
                                break;
                            case 2:
                                break;
                            case 4:
                                //�ҵ��յ�
                                isFind = true;
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            map[CurrentPoint.row - 1, CurrentPoint.col - 1].status = 2;
            if (isFind == true)
            {
                Clearmap();
                return true;
            }
            //ѡ����һ����Ϊ���·���Ľڵ�
            if (ExploreList.Count == 0)
            {
                Clearmap();
                return false;
            }
            else {
                CurrentPoint = ExploreList.Dequeue();
            }
        }
    }
    public void Clearmap() {
        for (int i = 1; i <= 7; i++)
        {
            for (int j = 1; j <= 12; j++)
            {
                if (map[i - 1, j - 1].status == 2 || map[i - 1, j - 1].status == 4) {
                    map[i - 1, j - 1].status = 0;
                }
            }
        }
        isFind = false;
        ExploreList.Clear();
    }
    public bool Navigation_Player(GameObject _go)
    {
        SetStatusBrick(_go);

        if (Multiple_Controller.Instance.is_Condition[0])
        {
            if (!isNavigation(Switch(Multiple_Controller.Instance._Player[0].transform), Switch(Multiple_Controller.Instance.EndPoint[0])))
            {
                DeleteStatus(_go);
                return false;
            }
        }
        else
        {
            if (!isNavigation(Switch(Multiple_Controller.Instance._Player[0].transform), Switch(Multiple_Controller.Instance.SetPoint[1].transform)) || !isNavigation(Switch(Multiple_Controller.Instance.SetPoint[1].transform), Switch(Multiple_Controller.Instance.EndPoint[0])))
            {
                DeleteStatus(_go);
                return false;
            }
        }
        if (Multiple_Controller.Instance.is_Condition[1])
        {
            if (!isNavigation(Switch(Multiple_Controller.Instance._Player[1].transform),Switch(Multiple_Controller.Instance.EndPoint[1])))
            {
                DeleteStatus(_go);
                return false;
            }
        }
        else
        {
            if (!isNavigation(Switch(Multiple_Controller.Instance._Player[1].transform), Switch(Multiple_Controller.Instance.SetPoint[0].transform)) || !isNavigation(Switch(Multiple_Controller.Instance.SetPoint[0].transform), Switch(Multiple_Controller.Instance.EndPoint[1])))
            {
                DeleteStatus(_go);
                return false;
            }
        }
        DeleteStatus(_go);
        return true;
    }
    public void SetStatusBrick(GameObject _go)
    {
        MyPoint myPoint = Switch(_go.transform);

        map[myPoint.row - 1, myPoint.col - 1].status = 1;
        _go.GetComponent<_platform>().status = MapCode.Brick;
    }
    public void SetStatusBan(GameObject _go)
    {
        MyPoint myPoint = Switch(_go.transform);

        map[myPoint.row - 1, myPoint.col - 1].status = 1;
        _go.GetComponent<_platform>().status = MapCode.Ban;
    }
    public void DeleteStatus(GameObject _go)
    {
        MyPoint myPoint = Switch(_go.transform);
        map[myPoint.row - 1, myPoint.col - 1].status = 0;
        _go.GetComponent<_platform>().status = MapCode.Default;
    }
    MyPoint Switch(Transform Point)
    {
        MyPoint vector2 = new();
        float row = Point.position.z / 1.3f + 1;
        float col = (Point.position.x - 0.75f * System.Math.Abs(4 - row) + 3.75f) / 1.5f;
        vector2.row = Mathf.RoundToInt(row);
        vector2.col = Mathf.RoundToInt(col);
        vector2.status = 0;
        return vector2;
    }
    public void ClearGrid() {
        for (int i = 1; i <= 7;i++) {
            for (int j = 1; j <= 12 - System.Math.Abs(i - 4); j++) {
                map[i - 1, j - 1].status = 0;
            }
        }
    }
}
