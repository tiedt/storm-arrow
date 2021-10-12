using System;

[System.Serializable]
public class ReturnRequest<T>
{
    
    public string status;
    public T data;

}
