/*
	Yelo: Open Sauce SDK
		Halo 1 (CE) Edition

	See license\OpenSauce\Halo1_CE for specific license information
*/
#pragma once

#if !PLATFORM_IS_DEDI

//#define EXTERNAL_SUBSYSTEM_SHADERS

namespace Yelo
{
	namespace Enums
	{
		enum pp_component_status : uint32
		{
			pp_component_status_initialised,
			pp_component_status_uninitialised,
			pp_component_status_initialisation_failed,

			_pp_component_status
		};
		enum pp_render_stage : uint32
		{
			pp_render_stage_pre_blur,
			pp_render_stage_pre_alpha_blended,
			pp_render_stage_pre_hud,
			pp_render_stage_pre_menu,
			pp_render_stage_post_menu,
			pp_render_stage_blur,

			_pp_render_stage,
		};
	};
	namespace Rasterizer { namespace PostProcessing
	{
		struct s_render_state_manager
		{
			struct s_state
			{
#ifdef API_DEBUG
				const char* m_state_name;
#endif
				D3DRENDERSTATETYPE m_state;
				DWORD m_stored_value;
				DWORD m_set_value;
			};

#ifdef API_DEBUG
			const char* m_state_set_name;
#endif
			s_state m_states[15];

			void StoreAndSet(IDirect3DDevice9* render_device);
			void RestoreStates(IDirect3DDevice9* render_device);
		};

		template<class T>
		struct s_component_toggle
		{
			BOOL is_ready;
			T* m_component;
		};

		template<class T>
		struct s_component_toggle_ref
		{
			uint32 component_index;
			T* m_component;
		};

		void		Initialize();
		void		Dispose();

		void		Initialize3D(IDirect3DDevice9* pDevice, D3DPRESENT_PARAMETERS* pParameters);
		void		OnLostDevice();
		void		OnResetDevice(D3DPRESENT_PARAMETERS* pParameters);
		void		Render();
		void		Release();

		void		LoadSettings(TiXmlElement* parent_element);
		void		SaveSettings(TiXmlElement* parent_element);

		void		InitializeForNewMap();
		void		DisposeFromOldMap();

		void		Update(real delta_time);
		
		void		RenderPreAlphaBlended();
		void		RenderPreHUD();
		void		RenderPreMenu();
		void		RenderPostMenu();

		void		LoadSystem();
		void		UnloadSystem();

		void		RenderSystems(Enums::pp_render_stage render_stage);
	};};
};
#endif