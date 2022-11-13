using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CreditsSectionData
{
    public string Title;
    public string Body;
    public string Image;
    public float Width;
    public float Height;
}

public class CreditsSection : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private TextMeshProUGUI _body;
    [SerializeField] private Image _image;

    public void SetData(CreditsSectionData data)
    {
        _title.gameObject.SetActive(false);
        _body.gameObject.SetActive(false);
        _image.gameObject.SetActive(false);

        if (data?.Title != null)
        {
            _title.gameObject.SetActive(true);
            _title.text = data.Title;
        }

        if (data?.Body != null)
        {
            _body.gameObject.SetActive(true);
            _body.text = data.Body;
        }

        if (data?.Image != null)
        {
            _image.gameObject.SetActive(true);
            _image.sprite = Resources.Load<Sprite>($"Sprites/{data.Image}");
            LayoutElement layoutElement = _image.GetComponent<LayoutElement>();
            layoutElement.minHeight = data.Height;
            layoutElement.minWidth = data.Width;
            layoutElement.preferredHeight = data.Height;
            layoutElement.preferredWidth = data.Width;
        }
    }
}
