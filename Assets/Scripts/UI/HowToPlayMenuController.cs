using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HowToPlayMenuController : MonoBehaviour
{
    public TextAsset resourceFile;
    public Image pageImage;
    public Text pageText;
    public Button nextPageButton;
    public Button prevPageButton;

    private TutorialPage[] pages;
    public int currentPage
    {
        get; private set;
    }

    void Awake()
    {
        Debug.Log(resourceFile.text);
        pages = JsonUtility.FromJson<TutorialPages>(resourceFile.text).pages;
        currentPage = 0;
        DisplayPage(pages[0]);
        SetPaginationButtonVisibility();
    }

    private void DisplayPage(TutorialPage page)
    {
        if (page.imagePath != null)
        {
            pageImage.sprite = Resources.Load<Sprite>(page.imagePath);
            Color imageColor = pageImage.color;
            imageColor.a = 255;
            pageImage.color = imageColor;
        }
        else
        {
            Color imageColor = pageImage.color;
            imageColor.a = 0;
            pageImage.color = imageColor;
        }
        pageText.text = page.description;
    }

    public void IncrementPage()
    {
        if (currentPage >= pages.Length - 1)
        {
            return;
        }
        currentPage++;
        DisplayPage(pages[currentPage]);
        SetPaginationButtonVisibility();
    }
    public void DecrementPage()
    {
        if (currentPage <= 0)
        {
            return;
        }
        currentPage--;
        DisplayPage(pages[currentPage]);
        SetPaginationButtonVisibility();
    }

    private void SetPaginationButtonVisibility()
    {
        if (currentPage <= 0)
        {
            prevPageButton.gameObject.SetActive(false);
        }
        else
        {
            prevPageButton.gameObject.SetActive(true);
        }
        if (currentPage >= pages.Length - 1)
        {
            nextPageButton.gameObject.SetActive(false);
        }
        else
        {
            nextPageButton.gameObject.SetActive(true);
        }
    }


}
