/*
using Firebase.Storage;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FirebaseFileDownloader : MonoBehaviour
{
    private FirebaseStorage storage;
    private string localFolderPath = "Assets/TestDataFolder";
    private List<string> firebaseFiles; // Firebase���� �ٿ�ε��� ���� ���

    void Start()
    {
        storage = FirebaseStorage.DefaultInstance;
        firebaseFiles = new List<string> { "���� ����.png" }; // �ٿ�ε��� ���� ��� ���

    }

    public void DownloadButtonClick()
    {
        // ���� ����� ��� �ִ��� Ȯ��
        if (firebaseFiles.Count == 0)
        {
            Debug.LogError("No files specified for download.");
            return; // FireBast Storage�� ���� ����� ��� ������ �ٿ�ε� �õ��� �ߴ�
        }

        DownloadFiles();
    }

    // ��� ������ �ٿ�ε��ϴ� �Լ�
    void DownloadFiles()
    {
        foreach (var firebaseFilePath in firebaseFiles)
        {
            string localFileName = Path.GetFileName(firebaseFilePath); // Firebase ���� ��ο��� ���� �̸� ����
            DownloadFile(firebaseFilePath, localFileName);
        }
    }

    // ������ �ٿ�ε��ϴ� �Լ�
    void DownloadFile(string firebaseFilePath, string localFileName)
    {

        string localFileFullPath = Path.Combine(localFolderPath, localFileName);

        // ���丮�� �������� �ʴ� ��� ����
        if (!Directory.Exists(localFolderPath))
        {
            Directory.CreateDirectory(localFolderPath);
        }

        // ���ÿ� ������ �̹� �����ϴ� ��� �ٿ�ε� �ߴ�
        if (File.Exists(localFileFullPath))
        {
            Debug.Log("File already exists and download skipped: " + localFileFullPath);
            return;
        }

        var storageRef = storage.GetReferenceFromUrl("gs://your-firebase-storage-url/" + firebaseFilePath);

        // Firebase Storage���� ������ �񵿱������� �ٿ�ε��ϰ� ���ÿ� ����
        storageRef.GetBytesAsync(long.MaxValue).ContinueWith(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError(task.Exception.ToString());  // ���� �߻� �� �α� ���
            }
            else
            {
                File.WriteAllBytes(localFileFullPath, task.Result);  // �ٿ�ε� ���� �����ͷ� ���� ����
                Debug.Log("File downloaded and saved to: " + localFileFullPath);  // ���� �α� ���
            }
        });
    }
}

*/
