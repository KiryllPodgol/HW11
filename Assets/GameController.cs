using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject blockPrefab;
    private GameObject _currentBlock;
    private GameObject _previousBlock;

    private const float Speed = 2.0f; // Скорость движения блока
    private float _direction = 1.0f; // Направление движения

    void Start()
    {
        CreateBlock(1f);
    }

    void Update()
    {
        if (_currentBlock != null)
        {
            // Двигаем текущий блок
            _currentBlock.transform.position += Vector3.right * (_direction * Speed * Time.deltaTime);

            if (_currentBlock.transform.position.x > 1.5f)
            {
                _direction = -1;
            }
            else if (_currentBlock.transform.position.x < -1.5f)
            {
                _direction = 1;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            float width = PlaceBlock();
            if (width > 0)
            {
                CreateBlock(width);
            }
        }
    }

    void CreateBlock(float width)
    {
        _currentBlock = Instantiate(blockPrefab);

        if (_previousBlock != null)
        {
            float previousHeight = _previousBlock.GetComponent<Renderer>().bounds.size.y;

            _currentBlock.transform.position = new Vector3(
                -1.5f,
                _previousBlock.transform.position.y + previousHeight,
                _previousBlock.transform.position.z
            );

            _currentBlock.GetComponent<MeshGeneration>().CutBlock(0, width);
            if (Camera.main != null) Camera.main.GetComponent<CameraController>().target = _currentBlock.transform;
        }
        else
        {
            _currentBlock.transform.position = Vector3.zero;
            
        }
    }
    
    float PlaceBlock()
    {
        if (_previousBlock == null)
        {
            _previousBlock = _currentBlock;
            return -1;
        }

        Bounds currentBounds = _currentBlock.GetComponent<Renderer>().bounds;
        Bounds previousBounds = _previousBlock.GetComponent<Renderer>().bounds;

        float left = Mathf.Max(currentBounds.min.x, previousBounds.min.x);
        float right = Mathf.Min(currentBounds.max.x, previousBounds.max.x);

        float overlap = right - left;

        Debug.Log($"Обработка блока: overlap = {overlap}, left = {left}, right = {right}");

        if (overlap > 0)
        {
            // Корректируем позицию текущего блока
            Vector3 correctedPosition = _currentBlock.transform.position;
            correctedPosition.x = (left + right) / 2;
            _currentBlock.transform.position = correctedPosition;

            // Обрезаем меш текущего блока
            float startOffset = left - currentBounds.min.x; // Смещение
            _currentBlock.GetComponent<MeshGeneration>().CutBlock(startOffset, overlap);

            // Создаем падающие куски
            if (currentBounds.max.x > right)
            {
                CreateFallingPiece(right, currentBounds.max.x, currentBounds);
            }

            if (currentBounds.min.x < left)
            {
                CreateFallingPiece(currentBounds.min.x, left, currentBounds);
            }

            _previousBlock = _currentBlock;

            Debug.Log($"Блок размещен: новая позиция = {correctedPosition}");

            return overlap;
        }
        else
        {
            Debug.Log("Игра окончена!");
            Time.timeScale = 0;
            return -1;
        }
        
    }
    void CreateFallingPiece(float left, float right, Bounds bounds)
    {
        GameObject fallingPiece = Instantiate(blockPrefab);

        fallingPiece.transform.position = new Vector3((left + right) / 2, bounds.center.y, bounds.center.z);

        Mesh mesh = fallingPiece.GetComponent<MeshFilter>().mesh;

        Vector3[] vertices = mesh.vertices;

        float width = right - left;

        for (int i = 0; i < vertices.Length; i++)
        {
            if (vertices[i].x > 0)
            {
                vertices[i].x = width / 2;
            }
            else
            {
                vertices[i].x = -width / 2;
            }
        }

        mesh.vertices = vertices;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        Rigidbody rb = fallingPiece.AddComponent<Rigidbody>();
        rb.useGravity = true;

        Destroy(fallingPiece, 3.0f);

        Debug.Log($"Падающий кусок создан с шириной: {width}");
    }
}