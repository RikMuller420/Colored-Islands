using System.Collections.Generic;
using SlimeGround.Data;
using SlimeGround.Data.ScriptableObjects.Paints;
using UnityEngine;

namespace SlimeGround.Gameplay.Islands
{
	public class IslandRenderer
	{
		public IslandRenderer(MeshRenderer renderer, ColorSampleMaterials paintMaterials)
	    {
	        _renderer = renderer;
	        _paintMaterials = paintMaterials;
	    }

		private ColorSampleMaterials _paintMaterials { get; }
		private MeshRenderer _renderer { get; }

		public void SetPaint(ColorSample colorSample, IReadOnlyCollection<SpriteRenderer> points)
	    {
	        foreach (ColorSampleMaterialData materials in _paintMaterials.Materials)
	        {
	            if (materials.ColorSample == colorSample)
	            {
	                _renderer.sharedMaterial = materials.IslandMaterial;

	                SetColor(materials.IslandPointColor, points);

	                return;
	            }
	        }
	    }

	    private void SetColor(Color color, IReadOnlyCollection<SpriteRenderer> points)
	    {
	        foreach (SpriteRenderer islandPoint in points)
	        {
	            islandPoint.color = color;
	        }
	    }
	}
}
