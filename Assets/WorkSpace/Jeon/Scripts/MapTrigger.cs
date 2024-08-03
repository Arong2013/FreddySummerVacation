using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Diagnostics;

public class MapTrigger : SerializedMonoBehaviour
{
    [ValueDropdown("GetAvailableMaps")]
    [SerializeField]
    private string mapNameToActivate;

    private IEnumerable<ValueDropdownItem<string>> GetAvailableMaps()
    {
        var mapManager = UiUtils.GetUI<MapManager>();
        if (mapManager != null)
        {
            return mapManager.GetAvailableMapNames()
                             .Select(name => new ValueDropdownItem<string>(name, name));
        }
        return new List<ValueDropdownItem<string>>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<Actor>())
        {
            if (!string.IsNullOrEmpty(mapNameToActivate))
            {
                UiUtils.GetUI<MapManager>().ChangeMap(mapNameToActivate);
            }
            else
            {
                Debug.LogWarning("No map name selected for this trigger.");
            }
        }
    }
}
