using UnityEngine;

public class Level : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}


	void OnTriggerEnter(Collider other){
		if (other.CompareTag("Player"))
		{
			QuitGame();
		}
	}

	void QuitGame()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false; // 在编辑器中停止播放
#else
		Application.Quit(); // 在打包后的应用中退出游戏
#endif
	}
}
