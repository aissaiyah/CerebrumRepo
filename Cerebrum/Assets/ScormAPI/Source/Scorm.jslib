var ScormFunctions = {
    $internal: {
        SCORM_TRUE: "true",
        SCORM_FALSE: "false",
        SCORM_NO_ERROR: "0",
        SCORM_1_2: "1_2",
        SCORM_2004: "2004",
        initialized: false,
        finishCalled: false,
        version: null,
        API: null,
        findAPITries: 0,
        maxAPITries: 10,

        Stringify: function (value) {
            return Pointer_stringify(value);
        },

        StringToUTF8: function (string) {
            var bufferSize = lengthBytesUTF8(string) + 1;
            var buffer = _malloc(bufferSize);
            
            stringToUTF8(string, buffer, bufferSize);
            
            return buffer;
        },

        /**
         * Find API.
         */
        FindAPI: function (win) {
            while ((this.version == this.SCORM_1_2 ? win.API == null : win.API_1484_11 == null) &&
                (win.parent != null) &&
                (win.parent != win)) {

                this.findAPITries++;

                if (this.findAPITries > this.maxAPITries) {
                    this.LogError("Error finding API, too deeply nested.");
                    return null;
                }

                win = win.parent;
            }

            return this.version == this.SCORM_1_2 ? win.API : win.API_1484_11;
        },

        /**
         * Get API.
         */
        GetAPI: function () {
            var theAPI = this.FindAPI(window);

            if ((theAPI == null) &&
                (window.opener != null) &&
                (typeof (window.opener) != "undefined")) {
                theAPI = this.FindAPI(window.opener);
            }

            if (theAPI == null) {
                this.LogError("Unable to find an API adapter");
            }

            return theAPI;
        },

        /**
         * Begins a communication session with the LMS.
         */
        Initialize: function () {
            var result;

            this.API = this.GetAPI();

            if (this.API == null) {
                this.LogError("Could not establish a connection with the LMS.\n\nYour results may not be recorded.");
                return;
            }

            switch (this.version) {
                case this.SCORM_1_2:
                    result = this.API.LMSInitialize("");
                    break;
                case this.SCORM_2004:
                    result = this.API.Initialize("");
                    break;
            }

            if (result == this.SCORM_FALSE) {
                var errorCode = this.GetLastError();
                var errorString = this.GetErrorString(errorCode);
                var diagnostic = this.GetDiagnostic(errorCode);

                var errorDescription = "Code: " + errorCode + "\nDescription: " + errorString + "\nDiagnostic: " + diagnostic;

                this.LogError("Could not initialize communication with the LMS.\n\nYour results may not be recorded.\n\n" + errorDescription);
                return false;
            } else {
                this.LogInfo("LMS communication initialized with SCORM Version: " + this.version);
            }

            this.initialized = true;

            return true;
        },

        /**
         * Indicates to the LMS that all data should be persisted (not required).
         */
        Commit: function () {
            var success = false;

            if (this.initialized == false || this.finishCalled == true) {
                return false;
            }

            switch (this.version) {
                case this.SCORM_1_2:
                    success = this.API.LMSCommit("");
                    break;
                case this.SCORM_2004:
                    success = this.API.Commit("");
                    break;
            }

            if (!success) {
                this.LogError("Couldn't commit.");

                return false;
            } else {
                return true;
            }
        },

        /**
         * Ends a communication session with the LMS.
         */
        Finish: function () {
            var result;

            if (this.initialized == false || this.finishCalled == true) {
                return false;
            }

            switch (this.version) {
                case this.SCORM_1_2:
                    result = this.API.LMSFinish("");
                    break;
                case this.SCORM_2004:
                    result = this.API.Terminate("");
                    break;
            }

            this.finishCalled = true;

            if (result == this.SCORM_FALSE) {
                var errorCode = this.GetLastError();
                var errorString = this.GetErrorString(errorCode);
                var diagnostic = this.GetDiagnostic(errorCode);

                var errorDescription = "Code: " + errorCode + "\nDescription: " + errorString + "\nDiagnostic: " + diagnostic;

                this.LogError("Could not terminate communication with the LMS.\n\nYour results may not be recorded.\n\n" + errorDescription);

                return false;
            } else {
                return true;
            }
        },

        /**
         * Gets a value.
         */
        GetValue: function (element) {
            var result;

            if (this.initialized == false || this.finishCalled == true) {
                return null;
            }

            switch (this.version) {
                case this.SCORM_1_2:
                    result = this.API.LMSGetValue(element);
                    break;
                case this.SCORM_2004:
                    result = this.API.GetValue(element);
                    break;
            }

            if (result == "") {
                var errorCode = this.GetLastError();

                if (errorCode != this.SCORM_NO_ERROR) {
                    var errorString = this.GetErrorString(errorCode);
                    var diagnostic = this.GetDiagnostic(errorCode);

                    var errorDescription = "Code: " + errorCode + "\nDescription: " + errorString + "\nDiagnostic: " + diagnostic;

                    this.LogError("Could not retrieve a value from the LMS.\n\n" + errorDescription);

                    return result
                }
            }

            return result;
        },

        /**
         * Set a value.
         */
        SetValue: function (element, value) {
            var result;

            if (this.initialized == false || this.finishCalled == true) {
                return false;
            }

            switch (this.version) {
                case this.SCORM_1_2:
                    result = this.API.LMSSetValue(element, value);
                    break;
                case this.SCORM_2004:
                    result = this.API.SetValue(element, value);
                    break;
            }

            if (result == this.SCORM_FALSE) {
                var errorCode = this.GetLastError();
                var errorString = this.GetErrorString(errorCode);
                var diagnostic = this.GetDiagnostic(errorCode);

                var errorDescription = "Code: " + errorCode + "\nDescription: " + errorString + "\nDiagnostic: " + diagnostic;

                this.LogError("Could not store a value in the LMS.\n\nYour results may not be recorded.\n\n" + errorDescription);

                return false;
            } else {
                return true;
            }
        },

        /**
         * Get last error.
         */
        GetLastError: function () {
            var errorCode;

            if (this.initialized == false || this.finishCalled == true) {
                return;
            }

            switch (this.version) {
                case this.SCORM_1_2:
                    errorCode = this.API.LMSGetLastError();
                    break;
                case this.SCORM_2004:
                    errorCode = this.API.GetLastError();
                    break;
            }

            return errorCode;
        },

        /**
         * Get error string.
         */
        GetErrorString: function (errorCode) {
            var errorString;

            if (this.initialized == false || this.finishCalled == true) {
                return;
            }

            switch (this.version) {
                case this.SCORM_1_2:
                    errorString = this.API.LMSGetErrorString(errorCode);
                    break;
                case this.SCORM_2004:
                    errorString = this.API.GetErrorString(errorCode);
                    break;
            }

            return errorString;
        },

        /**
         * Get diagnostic.
         */
        GetDiagnostic: function (errorCode) {
            var info;

            if (this.initialized == false || this.finishCalled == true) {
                return;
            }

            switch (this.version) {
                case this.SCORM_1_2:
                    info = this.API.LMSGetDiagnostic(errorCode);
                    break;
                case this.SCORM_2004:
                    info = this.API.GetDiagnostic(errorCode);
                    break;
            }

            return info;
        },

        /**
         * Logs an info message to the console.
         */
        LogInfo: function (text) {
            console.log("[SCORM - Info] " + text);
        },

        /**
         * Logs an error message to the console.
         */
        LogError: function (text) {
            console.log("[SCORM - Error] " + text);
        },

        /**
         * Converts from centiseconds to Timespan format.
         */
        FromCentisecondsToTimespan: function (centiseconds) {
            switch (this.version) {
                case this.SCORM_1_2:
                    return this.CentisecsToSCORM12Duration(centiseconds);
                case this.SCORM_2004:
                    return this.CentisecsToISODuration(centiseconds);
            }
        },

        /**
         * Converts from Timespan format to centiseconds.
         */
        FromTimespanToCentiseconds: function (timespan) {
            switch (this.version) {
                case this.SCORM_1_2:
                    return this.SCORM12DurationToCs(timespan);
                case this.SCORM_2004:
                    return this.ISODurationToCentisec(timespan);
            }
        },

        CentisecsToSCORM12Duration: function (n) {
            var bTruncated = false;
            
            with (Math) {
                n = round(n);
                var nH = floor(n / 360000);
                var nCs = n - nH * 360000;
                var nM = floor(nCs / 6000);
                nCs = nCs - nM * 6000;
                var nS = floor(nCs / 100);
                nCs = nCs - nS * 100;
            }

            if (nH > 9999) {
                nH = 9999;
                bTruncated = true;
            }

            var str = "0000" + nH + ":";
            str = str.substr(str.length-5, 5);

            if (nM < 10) str += "0";
            str += nM + ":";
            
            if (nS < 10) str += "0";
            str += nS;
            
            if (nCs > 0) {
                str += ".";
                if (nCs < 10) str += "0";
                str += nCs;
            }

            return str;
        },

        SCORM12DurationToCs: function (str) {
            var a = str.split(":");
            var nS = 0, n = 0;
            var nMult = 1;
            var bErr = ((a.length < 2) || (a.length > 3));
            
            if (!bErr) {
                for (var i = a.length - 1; i >= 0; i--) {
                    n = parseFloat(a[i]);

                    if (isNaN(n)) {
                        bErr = true;
                        break;
                    }

                    nS += n * nMult;
                    nMult *= 60;
                }
            }

            if (bErr) {
                console.log("Incorrect format: " + str + "\n\nFormat must be [HH]HH:MM:SS[.SS]");
                return NaN;
            }

            return Math.round(nS * 100);
        },

        CentisecsToISODuration: function (n, bPrecise) {
            var str = "P";
            var nCs = n;
            var nY = 0, nM = 0, nD = 0, nH = 0, nMin = 0, nS = 0;
            n = Math.max(n, 0);
            var nCs = n;

            with (Math) {
                nCs = round(nCs);

                if (bPrecise == true) {
                    nD = floor(nCs / 8640000);
                } else {
                    nY = floor(nCs / 3155760000);
                    nCs -= nY * 3155760000;
                    nM = floor(nCs / 262980000);
                    nCs -= nM * 262980000;
                    nD = floor(nCs / 8640000);
                }

                nCs -= nD * 8640000;
                nH = floor(nCs / 360000);
                nCs -= nH * 360000;
                var nMin = floor(nCs /6000);
                nCs -= nMin * 6000
            }
            
            if (nY > 0) str += nY + "Y";
            if (nM > 0) str += nM + "M";
            if (nD > 0) str += nD + "D";

            if ((nH > 0) || (nMin > 0) || (nCs > 0)) {
                str += "T";
                if (nH > 0) str += nH + "H";
                if (nMin > 0) str += nMin + "M";
                if (nCs > 0) str += (nCs / 100) + "S";
            }

            if (str == "P") str = "PT0H0M0S";

            return str;
        },

        ISODurationToCentisec: function (str) {
            var aV = new Array(0,0,0,0,0,0);
            var bErr = false;
            var bTFound = false;
            
            if (str.indexOf("P") != 0) bErr = true;
            
            if (!bErr) {
                var aT = new Array("Y","M","D","H","M","S")
                var p = 0, i = 0;

                str = str.substr(1); //get past the P

                for (i = 0 ; i < aT.length; i++) {
                    if (str.indexOf("T") == 0) {
                        str = str.substr(1);
                        i = Math.max(i, 3);
                        bTFound = true;
                    }

                    p = str.indexOf(aT[i]);

                    if (p > -1)
                    {
                        if ((i == 1) && (str.indexOf("T") > -1) && (str.indexOf("T") < p)) continue;
                        
                        if (aT[i] == "S") {
                            aV[i] = parseFloat(str.substr(0,p))
                        } else {
                            aV[i] = parseInt(str.substr(0,p))
                        }

                        if (isNaN(aV[i])) {
                            bErr = true;
                            break;
                        } else if ((i > 2) && (!bTFound)) {
                            bErr = true;
                            break;
                        }

                        str = str.substr(p+1);
                    }
                }

                if ((!bErr) && (str.length != 0)) bErr = true;
            }

            if (bErr) {
                return;
            }
            
            return aV[0]*3155760000 + aV[1]*262980000
                 + aV[2]*8640000 + aV[3]*360000 + aV[4]*6000
                 + Math.round(aV[5]*100);
        }
    },

    /**
     * Initialize version.
     */
    Scorm_Initialize: function(version) {
        var versionStr = internal.Stringify(version);

        if (versionStr == internal.SCORM_1_2 || versionStr == internal.SCORM_2004) {
            internal.version = versionStr;

            return internal.Initialize();
        } else {
            internal.LogError("Version not valid: " + versionStr);

            return false;
        }
    },

    /**
     * Indicates to the LMS that all data should be persisted.
     */
    Scorm_Commit: function() {
        return internal.Commit();
    },

    /**
     * Ends a communication session with the LMS.
     */
    Scorm_Finish: function() {
        return internal.Finish();
    },

    /**
     * Get a string value.
     */
    Scorm_GetStringValue: function(element) {
        var elementStr = internal.Stringify(element);

        var result = internal.GetValue(elementStr);

        return internal.StringToUTF8(result);
    },

    /**
     * Set a string value.
     */
    Scorm_SetStringValue: function(element, value) {
        var elementStr = internal.Stringify(element);
        var valueStr = internal.Stringify(value);

        return internal.SetValue(elementStr, valueStr);
    },

    /**
     * Get a int value.
     */
    Scorm_GetIntValue: function(element) {
        var elementStr = internal.Stringify(element);

        return internal.GetValue(elementStr);
    },

    /**
     * Set a int value.
     */
    Scorm_SetIntValue: function(element, value) {
        var elementStr = internal.Stringify(element);

        return internal.SetValue(elementStr, value);
    },

    /**
     * Get a float value.
     */
    Scorm_GetFloatValue: function(element) {
        var elementStr = internal.Stringify(element);

        return internal.GetValue(elementStr);
    },

    /**
     * Set a float value.
     */
    Scorm_SetFloatValue: function(element, value) {
        var elementStr = internal.Stringify(element);

        return internal.SetValue(elementStr, value);
    },

    /**
     * Identifies the learner on behalf of whom the SCO was launched.
     */
    Scorm_GetLearnerId: function () {
        switch (internal.version) {
            case internal.SCORM_1_2:
                var result = internal.GetValue("cmi.core.student_id");

                return internal.StringToUTF8(result);
            case internal.SCORM_2004:
                var result = internal.GetValue("cmi.learner_id");

                return internal.StringToUTF8(result);
        }
    },

    /**
     * Name provided for the learner by the LMS.
     */
    Scorm_GetLearnerName: function () {
        switch (internal.version) {
            case internal.SCORM_1_2:
                var result = internal.GetValue("cmi.core.student_name");

                return internal.StringToUTF8(result);
            case internal.SCORM_2004:
                var result = internal.GetValue("cmi.learner_name");

                return internal.StringToUTF8(result);
        }
    },

    /**
     * The learner’s current location in the SCO.
     */
    Scorm_GetLessonLocation: function () {
        switch (internal.version) {
            case internal.SCORM_1_2:
                var result = internal.GetValue("cmi.core.lesson_location");

                return internal.StringToUTF8(result);
            case internal.SCORM_2004:
                var result = internal.GetValue("cmi.location");

                return internal.StringToUTF8(result);
        }
    },

    /**
     * The learner’s current location in the SCO.
     */
    Scorm_SetLessonLocation: function (value) {
        var valueStr = internal.Stringify(value);

        switch (internal.version) {
            case internal.SCORM_1_2:
                return internal.SetValue("cmi.core.lesson_location", valueStr);
            case internal.SCORM_2004:
                return internal.SetValue("cmi.location", valueStr);
        }
    },

    /**
     * Indicates whether the learner will be credited for performance in the SCO.
     */
    Scorm_GetCredit: function () {
        switch (internal.version) {
            case internal.SCORM_1_2:
                var result = internal.GetValue("cmi.core.credit");

                return internal.StringToUTF8(result);
            case internal.SCORM_2004:
                var result = internal.GetValue("cmi.credit");

                return internal.StringToUTF8(result);
        }
    },

    /**
     * Indicates whether the learner has completed and satisfied the requirements for the SCO.
     */
    Scorm_GetLessonStatus: function () {
        switch (internal.version) {
            case internal.SCORM_1_2:
                var result = internal.GetValue("cmi.core.lesson_status");

                return internal.StringToUTF8(result);
            case internal.SCORM_2004:
                var result = internal.GetValue("cmi.completion_status");

                return internal.StringToUTF8(result);
        }
    },

    /**
     * Indicates whether the learner has completed and satisfied the requirements for the SCO.
     */
    Scorm_SetLessonStatus: function (value) {
        var valueStr = internal.Stringify(value);

        switch (internal.version) {
            case internal.SCORM_1_2:
                return internal.SetValue("cmi.core.lesson_status", valueStr);
            case internal.SCORM_2004:
                return internal.SetValue("cmi.completion_status", valueStr);
        }
    },

    /**
     * Asserts whether the learner has previously accessed the SCO.
     */
    Scorm_GetEntry: function () {
        switch (internal.version) {
            case internal.SCORM_1_2:
                var result = internal.GetValue("cmi.core.entry");

                return internal.StringToUTF8(result);
            case internal.SCORM_2004:
                var result = internal.GetValue("cmi.entry");

                return internal.StringToUTF8(result);
        }
    },

    /**
     * Number that reflects the performance of the learner relative to the range bounded by the values of min and max.
     */
    Scorm_GetRawScore: function () {
        switch (internal.version) {
            case internal.SCORM_1_2:
                return internal.GetValue("cmi.core.score.raw");
            case internal.SCORM_2004:
                return internal.GetValue("cmi.score.raw");
        }
    },

    /**
     * Number that reflects the performance of the learner relative to the range bounded by the values of min and max.
     */
    Scorm_SetRawScore: function (value) {
        switch (internal.version) {
            case internal.SCORM_1_2:
                return internal.SetValue("cmi.core.score.raw", value);
            case internal.SCORM_2004:
                return internal.SetValue("cmi.score.raw", value);
        }
    },

    /**
     * Maximum value in the range for the raw score.
     */
    Scorm_GetMaxScore: function () {
        switch (internal.version) {
            case internal.SCORM_1_2:
                return internal.GetValue("cmi.core.score.max");
            case internal.SCORM_2004:
                return internal.GetValue("cmi.score.max");
        }
    },

    /**
     * Maximum value in the range for the raw score.
     */
    Scorm_SetMaxScore: function (value) {
        switch (internal.version) {
            case internal.SCORM_1_2:
                return internal.SetValue("cmi.core.score.max", value);
            case internal.SCORM_2004:
                return internal.SetValue("cmi.score.max", value);
        }
    },

    /**
     * Minimum value in the range for the raw score.
     */
    Scorm_GetMinScore: function () {
        switch (internal.version) {
            case internal.SCORM_1_2:
                return internal.GetValue("cmi.core.score.min");
            case internal.SCORM_2004:
                return internal.GetValue("cmi.score.min");
        }
    },

    /**
     * Minimum value in the range for the raw score.
     */
    Scorm_SetMinScore: function (value) {
        switch (internal.version) {
            case internal.SCORM_1_2:
                return internal.SetValue("cmi.core.score.min", value);
            case internal.SCORM_2004:
                return internal.SetValue("cmi.score.min", value);
        }
    },

    /**
     * Sum of all of the learner’s session times accumulated in the current learner attempt.
     */
    Scorm_GetTotalTime: function () {
        var value; // in an attempt

        switch (internal.version) {
            case internal.SCORM_1_2:
                value = internal.GetValue("cmi.core.total_time");
                break;
            case internal.SCORM_2004:
                value = internal.GetValue("cmi.total_time");
                break;
        }

        return internal.FromTimespanToCentiseconds(value);
    },

    /**
     * Identifies one of three possible modes in which the SCO may be presented to the learner.
     */
    Scorm_GetLessonMode: function () {
        switch (internal.version) {
            case internal.SCORM_1_2:
                var result = internal.GetValue("cmi.core.lesson_mode");

                return internal.StringToUTF8(result);
            case internal.SCORM_2004:
                var result = internal.GetValue("cmi.mode");

                return internal.StringToUTF8(result);
        }
    },

    /**
     * Indicates how or why the learner left the SCO.
     */
    Scorm_SetExitReason: function (value) {
        var valueStr = internal.Stringify(value);

        switch (internal.version) {
            case internal.SCORM_1_2:
                return internal.SetValue("cmi.core.exit", valueStr);
            case internal.SCORM_2004:
                return internal.SetValue("cmi.exit", valueStr);
        }
    },

    /**
     * Amount of time that the learner has spent in the current learner session for this SCO.
     */
    Scorm_SetSessionTime: function (centiseconds) {
        switch (internal.version) {
            case internal.SCORM_1_2:
                return internal.SetValue("cmi.core.session_time", internal.FromCentisecondsToTimespan(centiseconds));
            case internal.SCORM_2004:
                return internal.SetValue("cmi.session_time", internal.FromCentisecondsToTimespan(centiseconds));
        }
    },

    /**
     * Provides space to store and retrieve data between learner sessions.
     */
    Scorm_GetSuspendData: function () {
        switch (internal.version) {
            case internal.SCORM_1_2:
                var result = internal.GetValue("cmi.suspend_data");

                return internal.StringToUTF8(result);
            case internal.SCORM_2004:
                var result = internal.GetValue("cmi.suspend_data");

                return internal.StringToUTF8(result);
        }
    },

    /**
     * Provides space to store and retrieve data between learner sessions.
     */
    Scorm_SetSuspendData: function (value) {
        var valueStr = internal.Stringify(value);

        switch (internal.version) {
            case internal.SCORM_1_2:
                return internal.SetValue("cmi.suspend_data", valueStr);
            case internal.SCORM_2004:
                return internal.SetValue("cmi.suspend_data", valueStr);
        }
    },

    /**
     * Textual input from the learner about the SCO.
     */
    Scorm_GetComments: function () {
        switch (internal.version) {
            case internal.SCORM_1_2:
                var result = internal.GetValue("cmi.comments");

                return internal.StringToUTF8(result);
            case internal.SCORM_2004:
                var result = internal.GetValue("cmi.comments_from_learner._children");

                return internal.StringToUTF8(result);
        }
    },

    /**
     * Comments or annotations associated with a SCO.
     */
    Scorm_GetCommentsFromLMS: function () {
        switch (internal.version) {
            case internal.SCORM_1_2:
                var result = internal.GetValue("cmi.comments_from_lms");

                return internal.StringToUTF8(result);
            case internal.SCORM_2004:
                var result = internal.GetValue("cmi.comments_from_lms._children");

                return internal.StringToUTF8(result);
        }
    },

    /**
     * Get the objectives.
     */
    Scorm_GetObjectives: function () {
        var objectives = [];

        for (var index = 0; index < internal.GetValue("cmi.objectives._count"); index++) {
            var objective = {};

            switch (internal.version) {
                case internal.SCORM_1_2:
                    objective.id = internal.GetValue("cmi.objectives." + index + ".id");
                    objective.minScore = internal.GetValue("cmi.objectives." + index + ".score.min");
                    objective.maxScore = internal.GetValue("cmi.objectives." + index + ".score.max");
                    objective.rawScore = internal.GetValue("cmi.objectives." + index + ".score.raw");

                    objective.successStatus = internal.GetValue("cmi.objectives." + index + ".status");

                    objectives.push(objective);
                    break;
                case internal.SCORM_2004:
                    objective.id = internal.GetValue("cmi.objectives." + index + ".id");
                    objective.minScore = internal.GetValue("cmi.objectives." + index + ".score.min");
                    objective.maxScore = internal.GetValue("cmi.objectives." + index + ".score.max");
                    objective.rawScore = internal.GetValue("cmi.objectives." + index + ".score.raw");

                    objective.scaledScore = internal.GetValue("cmi.objectives." + index + ".score.scaled");
                    objective.successStatus = internal.GetValue("cmi.objectives." + index + ".success_status");
                    objective.completionStatus = internal.GetValue("cmi.objectives." + index + ".completion_status");

                    objective.progressMeasure = internal.GetValue("cmi.objectives." + index + ".progress_measure");
                    objective.description = internal.GetValue("cmi.objectives." + index + ".description");

                    objectives.push(objective);
                    break;
            }
        }

        var result = JSON.stringify(objectives);

        return internal.StringToUTF8(result);
    },

    /**
     * Set a specific objective.
     */
    Scorm_SetObjective: function(data) {
        data = internal.Stringify(data);
        data = JSON.parse(data);

        var id = data.id.replace(/[^a-z0-9]/gi, '');
        var minScore = data.minScore;
        var maxScore = data.maxScore;
        var rawScore = data.rawScore;
        var scaledScore = data.scaledScore;
        var successStatus = data.successStatus;
        var completionStatus = data.completionStatus;
        var progressMeasure = data.progressMeasure;
        var description = data.description;

        var objectiveCount = internal.GetValue("cmi.objectives._count");

        var found;

        for (var i = 0; i < objectiveCount; i++) {
            if (internal.GetValue("cmi.objectives." + i + ".id") == id) {
                found = true;

                break;
            }
        }

        var index;

        if (found) {
            index = i;
        } else {
            index = objectiveCount;
        }

        switch (internal.version) {
            case internal.SCORM_1_2:
                internal.SetValue("cmi.objectives." + index + ".id", id);
                internal.SetValue("cmi.objectives." + index + ".score.min", minScore);
                internal.SetValue("cmi.objectives." + index + ".score.max", maxScore);
                internal.SetValue("cmi.objectives." + index + ".score.raw", rawScore);

                internal.SetValue("cmi.objectives." + index + ".status", successStatus);
                break;
            case internal.SCORM_2004:
                internal.SetValue("cmi.objectives." + index + ".id", id);
                internal.SetValue("cmi.objectives." + index + ".score.min", minScore);
                internal.SetValue("cmi.objectives." + index + ".score.max", maxScore);
                internal.SetValue("cmi.objectives." + index + ".score.raw", rawScore);

                internal.SetValue("cmi.objectives." + index + ".score.scaled", scaledScore);

                if (successStatus != "passed" && successStatus != "failed" && successStatus != "unknown") {
                    internal.LogError("Invalid 'success_status' value: '" + successStatus + "'. Possible values: passed, failed, unknown.");
                }

                internal.SetValue("cmi.objectives." + index + ".success_status", successStatus);

                if (completionStatus != "completed" && completionStatus != "incomplete" && completionStatus != "not attempted" && completionStatus != "unknown") {
                    internal.LogError("Invalid 'completion_status' value: '" + completionStatus + "'. Possible values: passed, failed, unknown.");
                }

                internal.SetValue("cmi.objectives." + index + ".completion_status", completionStatus);

                internal.SetValue("cmi.objectives." + index + ".progress_measure", progressMeasure);
                internal.SetValue("cmi.objectives." + index + ".description", description);
                break;
        }
    },

    /**
     * The learner’s preferred language for SCOs with multilingual capability.
     */
    Scorm_GetLanguage: function () {
        switch (internal.version) {
            case internal.SCORM_1_2:
                var result = internal.GetValue("cmi.student_preference.language");

                return internal.StringToUTF8(result);
            case internal.SCORM_2004:
                var result = internal.GetValue("cmi.learner_preference.language");

                return internal.StringToUTF8(result);
        }
    }
};

autoAddDeps(ScormFunctions, '$internal');
mergeInto(LibraryManager.library, ScormFunctions);