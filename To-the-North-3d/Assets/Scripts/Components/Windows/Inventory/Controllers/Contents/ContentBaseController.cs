using UnityEngine;

namespace Assets.Scripts.Components.Windows.Inventory
{
    public class ContentBaseController : MonoBehaviour
    {
        public Transform Container
        {
            get
            {
                return transform.parent.parent;
            }
        }
        public ContentType ContentType
        {
            get
            {
                return Container.GetComponent<ContainerBaseController>().ContentType;
            }
        }
    }
}
