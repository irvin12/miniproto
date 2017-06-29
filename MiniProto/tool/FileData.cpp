#include "FileData.h"

#include <string.h>
#include <fstream>

namespace common{
	namespace tool{

		FileData::FileData()
		{
			m_DataLen = 0;
		}
		FileData::FileData(const FileData& other)
		{
			*this = other;
		}
		FileData::~FileData()
		{

		}

		FileData& FileData::operator=(const FileData& other)
		{
			m_DataLen = other.m_DataLen;
			memcpy(m_Data, other.m_Data, other.m_DataLen);

			return *this;
		}

		void FileData::GetData(std::string& data, unsigned int begin, unsigned int end)
		{
			if (begin <= end && end < MaxDataLen)
			{
				data.assign(m_Data + begin, end - begin + 1);
			}
		}

		void FileData::SetData(const std::string& data, unsigned int begin, unsigned int end)
		{
			if (begin <= end && end < MaxDataLen && (end - begin + 1) <= data.length())
			{
				memcpy(m_Data + begin, data.c_str(), end - begin + 1);

				if (m_DataLen < end + 1)
				{
					m_DataLen = end + 1;
				}
			}
		}

		unsigned int FileData::GetLen()
		{
			return m_DataLen;
		}

		void FileData::SetLen(unsigned int len)
		{
			m_DataLen = len;
		}

		bool FileData::Load(const std::string& fileName)
		{
			std::ifstream file;
			file.open(fileName.c_str(), std::ios::in | std::ios::binary | std::ios::ate);
			if (file)
			{
				m_DataLen = static_cast<unsigned int>(file.tellg());
				if (m_DataLen <= MaxDataLen)
				{
					file.seekg(0, std::ios::beg);
					memset(m_Data, 0x00, sizeof(m_Data));
					file.read(m_Data, m_DataLen);
				}
				file.close();

				return true;
			}
			else
			{
				return false;
			}
		}

		bool FileData::Save(const std::string& fileName)
		{
			std::ofstream file;
			file.open(fileName.c_str(), std::ios::out | std::ios::trunc | std::ios::binary);
			if (file)
			{
				file.write(m_Data, m_DataLen);
				file.flush();
				file.close();

				return true;
			}
			else
			{
				return false;
			}
		}

	}
}