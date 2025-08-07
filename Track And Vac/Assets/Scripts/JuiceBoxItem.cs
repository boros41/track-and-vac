using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JuiceBoxItem : MonoBehaviour, IInteractable
{
    private const int SPEED_BUFF = 4;
    private const float DEFAULT_SPEED = 2f;
    private const int DEFAULT_FOV = 60;
    private const int SPEED_FOV = 80;

    private float _duration = 10f;
    private bool _isBuffActive;

    [SerializeField] private Image _background;
    [SerializeField] private TextMeshProUGUI _interactPrompt;
    [SerializeField] private TextMeshProUGUI _interactText;

    public string GetDescription()
    {
        return "Drink Juice";
    }

    public void Interact()
    {

        //SoundManager.Instance.SpeedBuffSound.Play();
        SoundManager.SoundToSource[SoundManager.Sound.SPEED_BUFF_PICKUP].Play();

        PlayerMovement.Instance.MovementSpeed = SPEED_BUFF;
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().fieldOfView = SPEED_FOV;

        PlayerMovement.Instance.StartCoroutine(RemoveBuffAfterDelay());

        Destroy(gameObject);
    }

    private IEnumerator RemoveBuffAfterDelay()
    {
        yield return new WaitForSeconds(5f);
        PlayerMovement.Instance.MovementSpeed = DEFAULT_SPEED;
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().fieldOfView = DEFAULT_FOV;
    }


    /*void Update()
    {
        if (_isBuffActive)
        {
            CountDownBuff();
        }
    }

    public string GetDescription()
    {
        if (!IsBuffOver())
        {
            //EnableInteractText(true);

            //return "Drink Juice";
        }
        else if (IsBuffOver())
        {
            //EnableInteractText(false);
        }

        return "Drink Juice";


        //return null;
    }

    public void Interact()
    {
        if (!_isBuffActive)
        {
            _isBuffActive = true;
            EnableInteractText(false);


            SoundManager.Instance.SpeedBuffSound.Play();
            PlayerMovement.Instance.MovementSpeed = SPEED_BUFF;
        }
    }

    private void CountDownBuff()
    {
        // hide juice instead of destroy initially or else the audio won't play
        GetComponent<MeshRenderer>().enabled = false;


        _duration -= Time.deltaTime;

        if (IsBuffOver())
        {
            _isBuffActive = false;
            EnableInteractText(true);

            PlayerMovement.Instance.MovementSpeed = DEFAULT_SPEED;
            Destroy(gameObject);
        }
    }

    private bool IsBuffOver()
    {
        return _duration <= 0;
    }

    private void EnableInteractText(bool flag)
    {
        _background.enabled = flag;
        _interactPrompt.enabled = flag;
        _interactText.enabled = flag;
    }*/


}
