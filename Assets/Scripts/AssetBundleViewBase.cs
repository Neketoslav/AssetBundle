using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AssetBundleViewBase : MonoBehaviour
{
    private const string UrlAssetBunldeSprites = "https://drive.google.com/uc?export=download&id=11bVizQGWzgC3Ks7F77zcj7ZxHzGp9R_J";
    private const string UrlAssetBunldeAudio = "https://drive.google.com/uc?export=download&id=1TleUG3Epvq47LGBfLeTwhnh_In5qAqj7";

    [SerializeField]
    private DataSpriteBundle[] _dataSpriteBundles;

    [SerializeField]
    private DataAudioBundle[] _dataAudioBundles;


    private AssetBundle _spriteAssetBundle;
    private AssetBundle _audioAssetBundle;

    protected IEnumerator DownloadAndSetAssetBundle()
    {
        yield return GetSpritesAssetBunlde();
        yield return GetAudioAssetBunlde();

        if(_spriteAssetBundle == null || _audioAssetBundle == null)
        {
            Debug.LogError(_spriteAssetBundle);
            Debug.LogError(_audioAssetBundle);
        }

        SetDownloadAssets();
        yield return null;
    }


    private IEnumerator GetSpritesAssetBunlde()
    {
        var request = UnityWebRequestAssetBundle.GetAssetBundle(UrlAssetBunldeSprites);

        yield return request.SendWebRequest();

        while (!request.isDone)
            yield return null;

        StateRequest(request, ref _spriteAssetBundle);
    }
    private IEnumerator GetAudioAssetBunlde()
    {
        var request = UnityWebRequestAssetBundle.GetAssetBundle(UrlAssetBunldeAudio);

        yield return request.SendWebRequest();

        while (!request.isDone)
            yield return null;

        StateRequest(request, ref _audioAssetBundle);
    }

    private void StateRequest(UnityWebRequest request, ref AssetBundle AssetBundle)
    {
        if(request.error == null)
        {
            AssetBundle = DownloadHandlerAssetBundle.GetContent(request);
            Debug.Log("Complete");
        }
        else
        {
            Debug.LogError(request.error);
        }
    }

    private void SetDownloadAssets()
    {
        foreach(var data in _dataSpriteBundles)
        {
            data.Image.sprite = _spriteAssetBundle.LoadAsset<Sprite>(data.NameAssetBundle);
        }
        foreach (var data in _dataAudioBundles)
        {
            data.AudioSource.clip = _audioAssetBundle.LoadAsset<AudioClip>(data.NameAssetsBundle);

        }
    }

}

