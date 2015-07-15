using System;
using System.Collections.Generic;
using System.Linq;

namespace Enrollment
{
    class Program
    {
        static void Main(string[] args)
        {
            var random = new Random();

            // Create list of five (5) random courses...
            var courses = new List<Course>();

            for (var i = 0; i < 5; i++)
            {
                var courseIndex = i + 1;
                var title = string.Format("Course{0}", courseIndex);
                var maxEnrollment = random.Next(40, 60);
                var course = new Course(title, maxEnrollment);

                courses.Add(course);
            }

            // Create list of one hundred (100) students...
            var students = new List<Student>();

            for (var i = 0; i < 100; i++)
            {
                var studentIndex = i + 1;
                var firstName = string.Format("FirstName{0}", studentIndex);
                var lastName = string.Format("LastName{0}", studentIndex);
                var student = new Student(firstName, lastName);

                students.Add(student);
            }

            // Randomly assign each student to two courses...
            var enrollments = new List<Enrollment>();

            foreach (var student in students)
            {
                var studentCount = 0;

                while (studentCount < 2)
                {
                    var courseIndex = random.Next(5);
                    var course = courses[courseIndex];
                    var courseCount = enrollments.Count(e => e.Course == course);
                    var courseStudentEnrolled = enrollments.Count(e => e.Course == course && e.Student == student) > 0;

                    if (courseCount < course.MaxEnrollment && !courseStudentEnrolled)
                    {
                        var enrollment = new Enrollment() { Student = student, Course = course };
                        enrollments.Add(enrollment);
                    }

                    studentCount = enrollments.Count(e => e.Student == student);
                }
            }

            // Display the result by course in order of decreasing enrollment count...
            var enrollmentCourseGroups =
                from enrollment in enrollments
                group enrollment by enrollment.Course into enrollmentCourseGroup
                orderby enrollmentCourseGroup.Count() descending
                select enrollmentCourseGroup;

            foreach (var enrollmentCourseGroup in enrollmentCourseGroups)
            {
                var course = enrollmentCourseGroup.Key;
                var count = enrollmentCourseGroup.Count();

                Console.WriteLine("{0}, Current Enrollment: {1} {2}", course, count, count == course.MaxEnrollment ? "CLOSED!" : "");

                foreach (var enrollmentCourse in enrollmentCourseGroup.OrderBy(e => e.Student.LastFirstName))
                {
                    Console.WriteLine("\t{0}", enrollmentCourse.Student);
                }

                Console.WriteLine();
            }

            Console.ReadLine();
        }
    }

    public class Course
    {
        private string _Title = "";
        private int _MaxEnrollment = 0;

        public string Title { get { return _Title; } }
        public int MaxEnrollment { get { return _MaxEnrollment; } }

        public Course(string title, int maxEnrollment)
        {
            _Title = title;
            _MaxEnrollment = maxEnrollment;
        }

        public override string ToString()
        {
            return string.Format("Title: {0}, Maximum Enrollment: {1}", _Title, _MaxEnrollment);
        }
    }

    public class Student
    {
        private string _FirstName = "";
        private string _LastName = "";

        public string FirstName { get { return _FirstName; } }
        public string LastName { get { return _LastName; } }
        public string FirstLastName { get { return string.Format("{0} {1}", _FirstName, _LastName); } }
        public string LastFirstName { get { return string.Format("{1}, {0}", _FirstName, _LastName); } }

        public Student(string firstName, string lastName)
        {
            _FirstName = firstName;
            _LastName = lastName;
        }

        public override string ToString()
        {
            return string.Format("First Name: {0}, Last Name: {1}", _FirstName, _LastName);
        }
    }

    public class Enrollment
    {
        public Student Student { get; set; }
        public Course Course { get; set; }
    }
}
