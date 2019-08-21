using Entitas;
using Systems;
using UnityEngine;

public class SystemController : MonoBehaviour
{
	private Entitas.Systems systems;

	private bool isStart;

	private void Start()
	{
		Init();
	}

	public void Init()
	{
		if (!isStart)
		{
			isStart = true;
			Pool pool = Pools.pool;
			systems = CreateSystems(pool);
			systems.Initialize();
		}
	}

	private void Update()
	{
		systems.Execute();
	}

	private static Entitas.Systems CreateSystems(Pool pool)
	{
		Entitas.Systems systems = new Entitas.Systems();
		systems.Add(pool.CreateSystem<CreateBoardSystem>()).Add(pool.CreateSystem<CreateShapeSystem>()).Add(pool.CreateSystem<CreateShadowSystem>())
			.Add(pool.CreateSystem<WinBonusSystem>())
			.Add(pool.CreateSystem<SetBombSystem>())
			.Add(pool.CreateSystem<ClonePrefabSystem>())
			.Add(pool.CreateSystem<SetParentSystem>())
			.Add(pool.CreateSystem<SetLocalPositionSystem>())
			.Add(pool.CreateSystem<SetPositionSystem>())
			.Add(pool.CreateSystem<SetScaleSystem>())
			.Add(pool.CreateSystem<ScaleDirectionSystem>())
			.Add(pool.CreateSystem<SetSpriteSystem>())
			.Add(pool.CreateSystem<GridNameSystem>())
			.Add(pool.CreateSystem<SpriteFitBoxSystem>())
			.Add(pool.CreateSystem<DestroyGridSystem>())
			.Add(pool.CreateSystem<DestroySystem>())
			.Add(pool.CreateSystem<TestReactiveSystem>());
		return systems;
	}
}
