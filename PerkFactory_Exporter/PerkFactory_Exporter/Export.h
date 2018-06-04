#pragma once
#include <vector>
#include <iostream>
#include <fstream>
#include <list>
#include <string>
#define DllExport  __declspec( dllexport )
using namespace std;



class Exporter
{
public:
	Exporter();
	~Exporter();

	struct Attributes
	{
		int nrOfAttributes;		// number of ints 
		int nrOfFlags;			// number of bools
	};

	enum class AttributeEnum
	{
		//	LIFESTEAL			
		Lifesteal,				//	0

		//	RESISTANCES
		Physical,				//	1
		Water,					//	2
		Fire,					//	3
		Nature,					//	4
		
		//	STATS
		Str_Flat,				//	5
		Agi_Flat,				//	6
		Int_Flat,				//	7
		Hp_Flat,				//	8
		Str_multi,				//	9
		Agi_multi,				//	10
		Int_multi,				//	11
		Hp_multi,				//	12

		//	DAMAGE
		Damage_multi,			//	13
		MeleeRange_multi,		//	14
		MeleeDmg_multi,			//	15
		RangeDmg_multi,			//	16
		AtkSpd_multi,			//	17
		MeleeAtkSpd_multi,		//	18
		ConsDmg_Multi,			//	19
		ConsAtkSpd_multi,		//	20
		ConsMeleeDmg_Multi,		//	21
		ConsRangeDmg_multi,		//	22
		ConsMeleeAtkSpd_multi,	//	23
		ConsRangeAtkSpd_multi,	//	24
	
		//	SKILLS
		NrOfSkills,				//	25
		Cooldown,				//	26
		skill_dmg_multi,		//	27
	
		//	IMMUNITIES
		Heal_immune,			//	28
		Physical_immune,		//	29
		Water_immune,			//	30
		Fire_immune,			//	31
		Nature_immune,			//	32
		Slow_immune,			//	33
		Stun_immune,			//	34
		Magic_immune,			//	35
		Knockback_immune,		//	36
	
		//	WEAPON LOCKS			
		Melee_lock,				//	37
		Range_lock,				//	38
		Magic_lock,				//	39
		Water_lock,				//	40
		Fire_lock,				//	41
		Nature_lock				//	42
			
		

	};
	
	int	CheckBoxStart = 27;

	Attributes Header;
	vector<int> Values;
	vector<int> AttributeTypes;
	vector<int> ExportVector;

	
	DllExport void AddAttribute(int type, int value = 0);
	DllExport void RemoveAttribute(int type);
	DllExport void prepExport();
	DllExport void Export(string name);
	string enumToString(int myEnum);

private:

};

extern"C"
{
	DllExport void __stdcall Export2(const char *name, int nameLength,int Values[], int Types[], int Checks[],int CheckSize, int TypeSize,int condition);
}

