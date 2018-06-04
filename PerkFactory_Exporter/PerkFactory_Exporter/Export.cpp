#include "Export.h"
#include <Windows.h>


Exporter::Exporter()
{
}

Exporter::~Exporter()
{
}

void Exporter::AddAttribute(int type, int value)
{
	

	for (unsigned int i = 0; i < AttributeTypes.size(); i++)
	{
		if (AttributeTypes[i] == type)
		{
			// type already exists

			if (type < CheckBoxStart)
			{
				Values[i] = value;
			}
			
		}
		else
		{
			if (type < CheckBoxStart)
			{
				AttributeTypes.push_back(type);
				Values.push_back(value);
				Header.nrOfAttributes++;
			}
			else
			{
				AttributeTypes.push_back(type);
				Header.nrOfAttributes++;
			}
			
		}
	}
	




}
void Exporter::RemoveAttribute(int type)
{
	for (unsigned int i = 0; i < AttributeTypes.size(); i++)
	{
		if (AttributeTypes[i] == type)
		{
			AttributeTypes.erase(AttributeTypes.begin() + i);
			Values.erase(Values.begin() + i);
		}
	}
}

void Exporter::prepExport()
{

	for (unsigned int i = 0; i < Values.size(); i++)
	{
		ExportVector.push_back(AttributeTypes[i]);
		ExportVector.push_back(Values[i]);
	}
	for (unsigned int i = Values.size(); i < AttributeTypes.size(); i++)
	{
		ExportVector.push_back(AttributeTypes[i]);
	}
}

void Exporter::Export(string name)
{
	string fullExtension = name + ".prk";
	string AsciiExtension = name + ".txt";
	ofstream exportFile(fullExtension, ios::out, ios::binary);
	ofstream exportFileAscii(AsciiExtension, ios::out);

	
	exportFileAscii << "NrOfAttributes: " << Header.nrOfAttributes << endl;
	exportFileAscii << "NrOfFlags: " << Header.nrOfFlags << endl;

	for (size_t i = 0; i < Values.size(); i++)
	{
		string type = enumToString(AttributeTypes[i]);
		exportFileAscii << type.c_str() << ": " << Values[i];
	}

	exportFile.write(reinterpret_cast<char*>(&Header), sizeof(Header));

	for (size_t j = 0; j < ExportVector.size(); j++)
	{
		exportFile.write(reinterpret_cast<char*>(ExportVector.data()), sizeof(ExportVector[0] * ExportVector.size()));
	}

	
	exportFile.close();
	exportFileAscii.close();
}

string Exporter::enumToString(int myEnum)
{
	switch (myEnum)
	{
	case 0: return "Lifesteal %";
	case 1: return "Physical resistance % increase";
	case 2: return "Water resistance % increase";
	case 3:	return "Fire resistance % increase";
	case 4:	return "Nature resistance % inrease";
	case 5: return "Magic resistance % increase";
	case 6: return "Ranged resistance % increase";
	case 7:	return "Strength flat increase";
	case 8:	return "Agillity flat increase";
	case 9:	return "Intelligence flat increase";
	case 10: return "Health flat increase";
	case 11: return "Strength % increase";
	case 12: return "Intelligence % increase";
	case 13: return "Agility % increase";
	case 14: return "Health % increase";
	case 15: return "Damage % increase";
	case 16: return "Melee range % increase";
	case 17: return "Melee damage % increase";
	case 18: return "Range damage % increase";
	case 19: return "Attack speed % increase";
	case 20: return "Melee attack speed % increase";
	case 21: return "Consecutive damage % increase";
	case 22: return "Consecutive attack speed % increase";
	case 23: return "Consecutive melee damage % increase";
	case 24: return "Consecutive range damage % increase";
	case 25: return "Consecutive melee attack speed % increase";
	case 26: return "Consecutive range attack speed % increase";
	case 27: return	"Number of skills";
	case 28: return	"Skill cooldown";
	case 29: return "Adaptive skill cooldown";
	case 30: return	"Skill damage % increase";
	case 31: return "Movement speed % increase";
	case 32: return "Max HP % healing";
	case 33: return "Flat HP healing";
	case 34: return "Heal immune";
	case 35: return	"Physical immune";
	case 36: return	"Water immune";
	case 37: return	"Fire immune";
	case 38: return	"Nature immune";
	case 39: return	"Slow immune";
	case 40: return	"Stun immune";
	case 41: return	"Magic immune";
	case 42: return	"Knockback immune";
	case 43: return "Melee lock";
	case 44: return "Range lock";
	case 45: return "Magic lock";
	case 46: return "Water lock";
	case 47: return "Fire lock";
	case 48: return "Nature lock";

	default: return "Out of bounds";
			   
	}	//		   
}

extern "C" DllExport void __stdcall Export2(const char *name, int nameLength,int Values[], int Types[], int Checks[],int CheckSize, int TypeSize, int Condition)
{
	Exporter* MyExporter = new Exporter();

	
	string perkName(name);
	string fullExtension = perkName + ".prk";
	string AsciiExtension = perkName + ".txt";

	ofstream exportFileAscii(AsciiExtension);
	ofstream exportFile(fullExtension, ios::binary);
	
	exportFileAscii << "Perk Name: ";
	exportFileAscii.write(perkName.c_str(), nameLength);
	exportFileAscii << endl;
	 
	exportFileAscii << "CheckSize: " << CheckSize << endl;
	exportFileAscii << "TypeSize: " << TypeSize << endl;
	exportFileAscii << "Condition: " << Condition << endl;

	int nameSize = (int)perkName.size();
	exportFile.write(reinterpret_cast<const char*>(&nameSize), sizeof(int));//Size of the name so that it can be read from an ifstream procedure
	exportFile.write(perkName.c_str(), perkName.size());
	exportFile.write((char*)(&CheckSize), sizeof(CheckSize));
	exportFile.write((char*)(&TypeSize), sizeof(TypeSize));
	exportFile.write((char*)(&Condition), sizeof(Condition));
	//exportFile.write(reinterpret_cast<const char*>(&a), sizeof(int));
	

	for (size_t i = 0; i < (size_t)TypeSize; i++)
	{
		string TypeStr = MyExporter->enumToString(Types[i]);
		exportFileAscii << TypeStr << ": "<< Values[i] <<endl;
		exportFile.write((char*)(&Types[i]), sizeof(int));
		exportFile.write((char*)(&Values[i]), sizeof(int));

	}
	for (size_t j = 0; j < (size_t)CheckSize; j++)
	{
		string TypeStr = MyExporter->enumToString(Checks[j]);
		exportFileAscii << TypeStr << endl;
		exportFile.write((char*)(&Checks[j]), sizeof(int));
	}
}

	
