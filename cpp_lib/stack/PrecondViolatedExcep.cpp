//Created by Zhao Xie
//@date 10/23/2013
/**@file PrecondViolatedExcep.cpp*/
#include "PrecondViolatedExcep.h"

PrecondViolatedExcep::PrecondViolateExcep(const string& message):
       logic_error("Precondition Violated Exception: " + message)
{}