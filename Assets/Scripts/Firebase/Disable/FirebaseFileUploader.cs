/*
using Firebase;
using Firebase.Extensions;
using Firebase.Storage;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FirebaseFileUploader : MonoBehaviour
{
    private FirebaseStorage storage;  // Firebase Storage ���񽺿� �����ϱ� ���� ����
    private string localFolder = "Assets/TestDataFolder";  // ���ε��� ������ �ִ� ���� ���� ���

    void Start()
    {
        storage = FirebaseStorage.DefaultInstance;  // Firebase Storage �ν��Ͻ� �ʱ�ȭ
        if (!Directory.Exists(localFolder))
        {
            Debug.LogError("Local folder does not exist.");
            return;
        }
    }

    public void UploadButtonClick()
    {
        UploadDirectory(localFolder);  // ������ �������� ���� ���ε� ����
    }

    // ������ ���丮�� ��� ������ ���ε��ϴ� �Լ�
    void UploadDirectory(string directoryPath)
    {
        string[] files = Directory.GetFiles(directoryPath, "*", SearchOption.AllDirectories);  // ������ ����� ��� ������ ������

        // ������ �ϳ��� ���� ��� ����� �޽��� ���
        if (files.Length == 0)
        {
            Debug.LogWarning("No files found to upload in directory: " + directoryPath);
            return;
        }

        foreach (var file in files)
        {
            if (!File.Exists(file)) continue;  // ������ �������� ������ ���� ���Ϸ� �ǳʶ�
            UploadFile(file, Path.GetRelativePath(localFolder, file).Replace("\\", "/"));  // ������ ���ε��ϴ� �Լ� ȣ��
        }
    }

    // ���� ������ Firebase Storage�� ���ε��ϴ� �Լ�
    void UploadFile(string localFile, string firebaseFile)
    {
        
        var storageRef = storage.GetReferenceFromUrl("gs://your-firebase-storage-url/" + firebaseFile);  // Firebase Storage ���� ���� ��θ� ����

        // Firebase Storage���� ������ ��Ÿ�����͸� ��û�Ͽ� ���� ���� ���� Ȯ��
        storageRef.GetMetadataAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                // ������ �������� ������ ���ε� ����
                var fileBytes = File.ReadAllBytes(localFile);  // ���� ������ ����Ʈ �����͸� ����
                var metadata = new MetadataChange
                {
                    ContentType = "application/octet-stream"  // ������ ��Ÿ������ ���� (���⼭�� MIME Ÿ���� octet-stream���� ����)
                };
                var uploadTask = storageRef.PutBytesAsync(fileBytes, metadata);  // ���� �����Ϳ� ��Ÿ�����͸� �̿��� �񵿱������� ���ε�

                uploadTask.ContinueWith(uploadTask =>
                {
                    if (uploadTask.IsFaulted || uploadTask.IsCanceled)
                    {
                        Debug.LogError(uploadTask.Exception.ToString());  // ���ε� �� ���� �߻� �� �α� ���
                    }
                    else
                    {
                        Debug.Log("File uploaded successfully: " + firebaseFile);  // ���ε� ���� �� �α� ���
                    }
                });
            }
            else if (task.IsCompleted)
            {
                // ������ �̹� �����ϸ� ����� �޽��� ���
                Debug.Log("File already exists and will not be uploaded: " + firebaseFile);
            }
        });
    }
}

*/