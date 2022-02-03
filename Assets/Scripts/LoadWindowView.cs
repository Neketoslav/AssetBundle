using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class LoadWindowView : AssetBundleViewBase
{
    [SerializeField]
    private Button _loadAssetButton;

    [SerializeField]
    private Button _loadPrefabButton;

    [SerializeField]
    private AssetReference _loadPrefab;

    [SerializeField]
    private RectTransform _mouthRoot;

    private List<AsyncOperationHandle<GameObject>> addresablePrefabs = new List<AsyncOperationHandle<GameObject>>();

    private void Start()
    {
        _loadAssetButton.onClick.AddListener(LoadAsset);
        _loadPrefabButton.onClick.AddListener(CreateAddressablePrefab);
    }

    private void CreateAddressablePrefab()
    {
        var addresablePrefab = Addressables.InstantiateAsync(_loadPrefab, _mouthRoot);
        addresablePrefabs.Add(addresablePrefab);
    }

    private void OnDestroy()
    {
        _loadAssetButton.onClick.RemoveAllListeners();

        foreach(var addresablePrefab in addresablePrefabs)
        {
            Addressables.ReleaseInstance(addresablePrefab);
        }
        addresablePrefabs.Clear();
    }

    private void LoadAsset()
    {
        _loadAssetButton.interactable = false;

        StartCoroutine(DownloadAndSetAssetBundle());
    }
}
