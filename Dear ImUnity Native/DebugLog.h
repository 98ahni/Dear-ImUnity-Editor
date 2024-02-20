#pragma once
#include "UnityAPI/IUnityLog.h"
#include <string>

#define LogInfo __FUNCTION__, __FILE__, __LINE__
#define UNITY_ASSERT(expression) (void)(                                                       \
            (!!(expression)) ||                                                              \
            (::Debug::LogException("!Ngin assert! " #expression), 0) \
        )
#define UNITY_IM_ASSERT(expression) (void)(                                                       \
            (!!(expression)) ||                                                              \
            (::Debug::LogException("!IM_ASSERT! " #expression), 0) \
        )
#define UNITY_LOG_CODE(message, expression) expression \
		::Debug::Log(std::string(message + \
			std::string("\nCODE") + #expression "\n").c_str()\
		)

class Debug
{
	//class ImException : public std::exception
	//{
	//public:
	//	virtual const char* what() const throw()
	//	{
	//		return "ImException thrown!";
	//	}
	//};

public:
	static void Init(IUnityLog* aLogger)
	{
		myLogger = aLogger;
	}
	static void Log(const char* message, const char* funcName = __builtin_FUNCTION(), const char* fileName = __builtin_FILE(), const int fileLine = __builtin_LINE())
	{
		myLogger->Log(kUnityLogTypeLog, (message + std::string("\n(") + funcName + ") " + std::string(fileName).substr(std::string(fileName).find_last_of('\\')) + " : " + std::to_string(fileLine)).c_str(), fileName, fileLine);
	}
	static void LogWarning(const char* message, const char* funcName = __builtin_FUNCTION(), const char* fileName = __builtin_FILE(), const int fileLine = __builtin_LINE())
	{
		myLogger->Log(kUnityLogTypeWarning, (message + std::string("\n(") + funcName + ") " + std::string(fileName).substr(std::string(fileName).find_last_of('\\')) + " : " + std::to_string(fileLine)).c_str(), fileName, fileLine);
	}
	static void LogError(const char* message, const char* funcName = __builtin_FUNCTION(), const char* fileName = __builtin_FILE(), const int fileLine = __builtin_LINE())
	{
		myLogger->Log(kUnityLogTypeError, (message + std::string("\n(") + funcName + ") " + std::string(fileName).substr(std::string(fileName).find_last_of('\\')) + " : " + std::to_string(fileLine)).c_str(), fileName, fileLine);
	}
	static void LogException(const char* message, const char* funcName = __builtin_FUNCTION(), const char* fileName = __builtin_FILE(), const int fileLine = __builtin_LINE())
	{
		myLogger->Log(kUnityLogTypeException, (message + std::string("\n(") + funcName + ") " + std::string(fileName).substr(std::string(fileName).find_last_of('\\')) + " : " + std::to_string(fileLine)).c_str(), fileName, fileLine);
		//throw new ImException();
	}

private:
	static inline IUnityLog* myLogger;
};