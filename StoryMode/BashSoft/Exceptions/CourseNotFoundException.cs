using System;
using System.Collections.Generic;
using System.Text;

namespace BashSoft.Exceptions
{
    public class CourseNotFoundException : Exception
    {
        private const string courseNotFound = "Student must be enrolled in a course before you set his mark.";

        public CourseNotFoundException()
            : base(courseNotFound)
        {
        }

        public CourseNotFoundException(string message)
            : base(message)
        {
        }
    }
}
