using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ObjectStats<T> : MonoBehaviour where T: MonoBehaviour, IObjectable<T>
{
    [SerializeField] protected Spawner<T> _spawner;

    private int _objectCreated;
    private int _objectSpawned;
    private int _activeObjects;

    private TextMeshProUGUI _text;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
        UpdateText();
    }

    private void OnEnable()
    {
        _spawner.ObjectCreated += OnObjectCreated;
        _spawner.ObjectReceived += OnObjectReceived;
        _spawner.ObjectReleased += OnObjectReleased;
    }

    private void OnDisable()
    {
        _spawner.ObjectCreated -= OnObjectCreated;
        _spawner.ObjectReceived -= OnObjectReceived;
        _spawner.ObjectReleased -= OnObjectReleased;
    }

    private void OnObjectCreated()
    {
        _objectCreated++;
        UpdateText();
    }

    private void OnObjectReceived()
    {
        _objectSpawned++;
        _activeObjects++;
        UpdateText();
    }

    private void OnObjectReleased(T cube)
    {
        _activeObjects--;
        UpdateText();
    }

    private void UpdateText()
    {
        _text.text = $"Количество созданных объектов: {_objectCreated}\nКоличество заспавненных объектов: {_objectSpawned}\nКоличество активных объектов: {_activeObjects}";
    }
}
