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
    private List<string> firebaseFiles; // Firebase에서 다운로드할 파일 목록

    void Start()
    {
        storage = FirebaseStorage.DefaultInstance;
        firebaseFiles = new List<string> { "샘플 사진.png" }; // 다운로드할 파일 경로 목록

    }

    public void DownloadButtonClick()
    {
        // 파일 목록이 비어 있는지 확인
        if (firebaseFiles.Count == 0)
        {
            Debug.LogError("No files specified for download.");
            return; // FireBast Storage에 파일 목록이 비어 있으면 다운로드 시도를 중단
        }

        DownloadFiles();
    }

    // 모든 파일을 다운로드하는 함수
    void DownloadFiles()
    {
        foreach (var firebaseFilePath in firebaseFiles)
        {
            string localFileName = Path.GetFileName(firebaseFilePath); // Firebase 파일 경로에서 파일 이름 추출
            DownloadFile(firebaseFilePath, localFileName);
        }
    }

    // 파일을 다운로드하는 함수
    void DownloadFile(string firebaseFilePath, string localFileName)
    {

        string localFileFullPath = Path.Combine(localFolderPath, localFileName);

        // 디렉토리가 존재하지 않는 경우 생성
        if (!Directory.Exists(localFolderPath))
        {
            Directory.CreateDirectory(localFolderPath);
        }

        // 로컬에 파일이 이미 존재하는 경우 다운로드 중단
        if (File.Exists(localFileFullPath))
        {
            Debug.Log("File already exists and download skipped: " + localFileFullPath);
            return;
        }

        var storageRef = storage.GetReferenceFromUrl("gs://your-firebase-storage-url/" + firebaseFilePath);

        // Firebase Storage에서 파일을 비동기적으로 다운로드하고 로컬에 저장
        storageRef.GetBytesAsync(long.MaxValue).ContinueWith(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError(task.Exception.ToString());  // 에러 발생 시 로그 출력
            }
            else
            {
                File.WriteAllBytes(localFileFullPath, task.Result);  // 다운로드 받은 데이터로 파일 생성
                Debug.Log("File downloaded and saved to: " + localFileFullPath);  // 성공 로그 출력
            }
        });
    }
}

*/
