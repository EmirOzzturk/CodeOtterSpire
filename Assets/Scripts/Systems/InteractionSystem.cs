using Action_System;
using UnityEngine;

public class InteractionSystem : Singleton<InteractionSystem>
{
   public bool PlayerIsDragging { get; set; } = false;
   public bool IsModalOpen { get; set; } = false;

   public bool PlayerCanInteract()
   {
      return !ActionSystem.Instance.IsPreforming && !IsModalOpen;
   }

   public bool PlayerCanHover()
   {
      return !PlayerIsDragging && !IsModalOpen;
   }
}
