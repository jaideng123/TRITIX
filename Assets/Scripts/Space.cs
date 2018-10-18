using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Space : MonoBehaviour
{
    [SerializeField]
    private PlayerController playerController;
    private float heightOffset = 3;
    private float duration = .2f;
    private const string soundEffectName = "FOOTSTEP - Metal Board Walk Barefoot Male - 2";
    private AudioClip soundEffect;
    private AudioSource _audioSource;
    private GameObject _pieceObject;
    private GameObject _pieceTempObject;
    public Piece piece { get; private set; }
    private GameObject _touchBox;
    private bool _active;
    // Use this for initialization
    void Start()
    {
        _touchBox = GetComponentInChildren<TouchTarget>().gameObject;
        soundEffect = Resources.Load("Sounds/" + soundEffectName) as AudioClip;
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource != null)
        {
            _audioSource.clip = soundEffect;
            _audioSource.minDistance = 10;
            _audioSource.maxDistance = 500;
            _audioSource.pitch = .7f;
            _audioSource.playOnAwake = false;
        }
        piece = null;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Triggered()
    {
        BoardLayer layer = transform.GetComponentInParent<BoardLayer>();
        if (layer != null)
        {
            layer.SpaceSelected(this);
        }
        else
        {
            Debug.LogError("No Layer Object Found!");
        }
    }

    public void ApplyPiece(Piece newPiece)
    {
        piece = newPiece;
        if (_pieceObject != null)
        {
            Destroy(_pieceObject);
        }
        _pieceObject = Instantiate(Resources.Load("Pieces/Prefabs/" + piece.type.ToString()) as GameObject);
        _pieceObject.transform.SetParent(this.transform, false);
        Vector3 pos = _pieceObject.transform.localPosition;
        pos.y += heightOffset;
        _pieceObject.transform.localPosition = pos;
        StartCoroutine(LowerPiece(_pieceObject));
        string pieceMat = playerController.GetPlayer(piece.playerNum).pieceMaterialName;
        _pieceObject.GetComponent<Renderer>().material = Resources.Load("Pieces/Materials/" + pieceMat) as Material;
        _pieceObject.GetComponentInChildren<Outline>().SetVisible(false);
    }

    private IEnumerator LowerPiece(GameObject piece)
    {
        float target = piece.transform.localPosition.y - heightOffset;
        float starting = piece.transform.localPosition.y;
        float startTime = Time.time;

        while (piece.transform.localPosition.y > target)
        {
            float t = (Time.time - startTime) / duration;
            piece.transform.localPosition = new Vector3(0, Mathf.SmoothStep(starting, target, t), 0);
            yield return null;
        }
        if (_audioSource)
        {
            _audioSource.Play();
        }
    }

    public void ApplyPieceTemp(PieceType type)
    {
        if (_pieceTempObject != null)
        {
            Destroy(_pieceTempObject);
        }
        _pieceTempObject = Instantiate(Resources.Load("Pieces/Prefabs/" + type.ToString()) as GameObject);
        _pieceTempObject.transform.SetParent(this.transform, false);
        _pieceTempObject.GetComponent<Renderer>().material = Resources.Load("Pieces/Materials/TempPieceMat") as Material;
        _pieceTempObject.GetComponentInChildren<Outline>().SetVisible(false);
    }
    public void ClearPieceTemp()
    {
        if (_pieceTempObject != null)
        {
            Destroy(_pieceTempObject);
        }
    }

    public void ClearPiece()
    {
        piece = null;
        Destroy(_pieceObject);
    }

    public void SetActive(bool active)
    {
        if (active)
        {
            _touchBox.GetComponent<Renderer>().material = Resources.Load("Spaces/Materials/SpaceTargetActive") as Material;
        }
        else
        {
            _touchBox.GetComponent<Renderer>().material = Resources.Load("Spaces/Materials/SpaceTarget") as Material;
        }
        _active = active;
    }

    public void SetMatched(bool active)
    {
        if (_pieceObject == null)
        {
            Debug.LogWarning("No Piece Object Set");
            return;
        }
        Outline outline = _pieceObject.GetComponentInChildren<Outline>();
        if (active)
        {
            outline.SetVisible(true);
        }
        else
        {
            outline.SetVisible(false);
        }
    }
}
