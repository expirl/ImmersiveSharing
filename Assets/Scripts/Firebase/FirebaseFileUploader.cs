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
    private FirebaseStorage storage;  // Firebase Storage 서비스에 접근하기 위한 변수
    private string localFolder = "Assets/TestDataFolder";  // 업로드할 파일이 있는 로컬 폴더 경로

    void Start()
    {
        storage = FirebaseStorage.DefaultInstance;  // Firebase Storage 인스턴스 초기화
        if (!Directory.Exists(localFolder))
        {
            Debug.LogError("Local folder does not exist.");
            return;
        }
    }

    public void UploadButtonClick()
    {
        UploadDirectory(localFolder);  // 지정된 폴더에서 파일 업로드 시작
    }

    // 지정된 디렉토리의 모든 파일을 업로드하는 함수
    void UploadDirectory(string directoryPath)
    {
        string[] files = Directory.GetFiles(directoryPath, "*", SearchOption.AllDirectories);  // 지정된 경로의 모든 파일을 가져옴

        // 파일이 하나도 없을 경우 디버그 메시지 출력
        if (files.Length == 0)
        {
            Debug.LogWarning("No files found to upload in directory: " + directoryPath);
            return;
        }

        foreach (var file in files)
        {
            if (!File.Exists(file)) continue;  // 파일이 존재하지 않으면 다음 파일로 건너뜀
            UploadFile(file, Path.GetRelativePath(localFolder, file).Replace("\\", "/"));  // 파일을 업로드하는 함수 호출
        }
    }

    // 실제 파일을 Firebase Storage에 업로드하는 함수
    void UploadFile(string localFile, string firebaseFile)
    {
        
        var storageRef = storage.GetReferenceFromUrl("gs://your-firebase-storage-url/" + firebaseFile);  // Firebase Storage 내의 저장 경로를 설정

        // Firebase Storage에서 파일의 메타데이터를 요청하여 파일 존재 여부 확인
        storageRef.GetMetadataAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                // 파일이 존재하지 않으면 업로드 진행
                var fileBytes = File.ReadAllBytes(localFile);  // 로컬 파일의 바이트 데이터를 읽음
                var metadata = new MetadataChange
                {
                    ContentType = "application/octet-stream"  // 파일의 메타데이터 설정 (여기서는 MIME 타입을 octet-stream으로 설정)
                };
                var uploadTask = storageRef.PutBytesAsync(fileBytes, metadata);  // 파일 데이터와 메타데이터를 이용해 비동기적으로 업로드

                uploadTask.ContinueWith(uploadTask =>
                {
                    if (uploadTask.IsFaulted || uploadTask.IsCanceled)
                    {
                        Debug.LogError(uploadTask.Exception.ToString());  // 업로드 중 에러 발생 시 로그 출력
                    }
                    else
                    {
                        Debug.Log("File uploaded successfully: " + firebaseFile);  // 업로드 성공 시 로그 출력
                    }
                });
            }
            else if (task.IsCompleted)
            {
                // 파일이 이미 존재하면 디버그 메시지 출력
                Debug.Log("File already exists and will not be uploaded: " + firebaseFile);
            }
        });
    }
}

*/