using UnityEngine;
using System.Collections;

public class From
{
    public Transform star;
    public Portal portal;
    public From(Transform star, Portal portal)
    {
        this.star = star;
        this.portal = portal;
    }
}
public class Portals : MonoBehaviour
{
    public Portal portal1, portal2;
    void setPos(From from)
    {
        Portal portalFrom = from.portal,
            portalTo = (from.portal == portal1) ? portal2 : portal1;

        portalFrom.state = PORTAL.From;
        portalTo.state = PORTAL.To;
        Transform star = from.star;

        Vector2 v = star.position - portalFrom.transform.position;
        v = v / portalFrom.radius * portalTo.radius;

        star.position = portalTo.transform.position + (Vector3)v;
    }
}
