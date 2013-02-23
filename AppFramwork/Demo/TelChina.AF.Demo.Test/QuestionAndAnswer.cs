using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TelChina.AF.Persistant;

namespace TelChina.AF.Demo.Test
{
    [TestClass]
    public class QuestionAndAnswer
    {
        private TestContext testContextInstance;

        /// <summary>
        ///获取或设置测试上下文，上下文提供
        ///有关当前测试运行及其功能的信息。
        ///</summary>
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        [TestInitialize()]
        public void MyTestInitialize()
        {
            RepositoryContext.Config();

            ClearData();
        }

        private void ClearData()
        {
            using (var repo = RepositoryContext.GetRepository())
            {
                var answers = repo.GetAll<Answer>();
                
                foreach (var answer in answers)
                {
                    repo.Remove(answer);
                }

                var answers1 = repo.GetAll<Answer1>();

                foreach (var answer in answers1)
                {
                    repo.Remove(answer);
                }

                var answers2 = repo.GetAll<Answer2>();

                foreach (var answer in answers2)
                {
                    repo.Remove(answer);
                }

                var questions = repo.GetAll<Question>();

                foreach (var question in questions)
                {
                    repo.Remove(question);
                }

                var questions1 = repo.GetAll<Question1>();

                foreach (var question in questions1)
                {
                    repo.Remove(question);
                }

                var questions2 = repo.GetAll<Question2>();

                foreach (var question in questions2)
                {
                    repo.Remove(question);
                }

                repo.SaveChanges();
            }
        }

        /// <summary>
        /// 懒加载lazy = "true"测试        
        /// </summary>
        [TestMethod]
        public void LazyTrueloadTest()
        {
            Question question;
            using (var repo = RepositoryContext.GetRepository())
            {
                question = new Question() { Name = "question1123", CreatedOn = DateTime.Now, UpdatedOn = DateTime.Now };


                var addAnswer1 = new Answer() { Name = "answer1", CreatedOn = DateTime.Now, UpdatedOn = DateTime.Now, Question = question };
                var addAnswer2 = new Answer() { Name = "answer2", CreatedOn = DateTime.Now, UpdatedOn = DateTime.Now, Question = question };
                question.Answers.Add(addAnswer1);
                question.Answers.Add(addAnswer2);

                repo.Add(question);

                repo.SaveChanges();
            }

            using (var repo = RepositoryContext.GetRepository())
            {
                //SELECT * FROM [Question]
                var question1 = repo.GetAll<Question>().FirstOrDefault();
                //SELECT * FROM [Answer] answers0_ WHERE answers0_.Question_ID='59B9C112-2A62-4E41-8220-7A518D829449';
                var count = question1.Answers.Count;
                var beforeLoad = question1.GetAnswersSet();
                //Assert.IsTrue(beforeLoad == null || beforeLoad.Count == 0, "组合子实体被立即加载！");
                //Assert.AreEqual(question.Answers.Count, 0);
                var afterLoad = question1.Answers;
                Assert.IsTrue(beforeLoad != null && beforeLoad.Count > 0, "组合子实体未被加载出来！");
            }
        }

        /// <summary>
        /// 懒加载lazy = "extra"测试 
        /// 加载集合的数量时有所区别       
        /// </summary>
        [TestMethod]
        public void LazyExtraloadTest()
        {
            Question3 question;
            using (var repo = RepositoryContext.GetRepository())
            {
                question = new Question3() { Name = "question1123", CreatedOn = DateTime.Now, UpdatedOn = DateTime.Now };


                var addAnswer1 = new Answer3() { Name = "answer1", CreatedOn = DateTime.Now, UpdatedOn = DateTime.Now, Question = question };
                var addAnswer2 = new Answer3() { Name = "answer2", CreatedOn = DateTime.Now, UpdatedOn = DateTime.Now, Question = question };
                question.Answers.Add(addAnswer1);
                question.Answers.Add(addAnswer2);

                repo.Add(question);

                repo.SaveChanges();
            }

            using (var repo = RepositoryContext.GetRepository())
            {
                //SELECT * FROM [Question]
                var question1 = repo.GetAll<Question3>().FirstOrDefault();
                //SELECT count(ID) FROM [Answer] WHERE Question_ID='C421EA5F-89E3-4168-BF7D-60B8DB775C92'
                var count = question1.Answers.Count;
                //Assert.AreEqual(question.Answers.Count, 0);
                var afterLoad = question1.Answers;
            }
        }

        /// <summary>
        /// 懒加载的导航属性在兰姆达表达式中的测试
        /// </summary>
        [TestMethod]
        public void NavigationLazyLoadTest()
        {
            Question question;
            Guid answerID;
            // 添加数据记录
            using (var repo = RepositoryContext.GetRepository())
            {
                question = new Question() {Name = "question1123", CreatedOn = DateTime.Now, UpdatedOn = DateTime.Now};


                var addAnswer1 = new Answer()
                                     {
                                         Name = "answer1",
                                         CreatedOn = DateTime.Now,
                                         UpdatedOn = DateTime.Now,
                                         Question = question
                                     };
                var addAnswer2 = new Answer()
                                     {
                                         Name = "answer2",
                                         CreatedOn = DateTime.Now,
                                         UpdatedOn = DateTime.Now,
                                         Question = question
                                     };
                answerID = addAnswer1.ID;

                question.Answers.Add(addAnswer1);
                question.Answers.Add(addAnswer2);

                repo.Add(question);

                repo.SaveChanges();
            }

            // 由Question到Answer
            using (var repo = RepositoryContext.GetRepository())
            {
                // SELECT *FROM [Question] question0_ WHERE question0_.ID='18884214-A293-4EAE-A999-972334E6322A';
                var testQuestion = repo.GetByID<Question>(question.ID);

                // 导航属性SQL测试                
                //SELECT answers0_.Question_ID as Question8_1_, * FROM [Answer] answers0_ WHERE answers0_.Question_ID='18884214-A293-4EAE-A999-972334E6322A';
                //SELECT * FROM [Answer] this_ WHERE this_.Question_ID = '18884214-A293-4EAE-A999-972334E6322A';
                var answer =
                    repo.GetAll<Answer>(p => p.Question_ID == testQuestion.Answers.FirstOrDefault().Question.ID);

                // 单元测试插入了两条answer数据
                Assert.AreEqual(answer[0].Question_ID, testQuestion.ID);
                Assert.AreEqual(answer[1].Question_ID, testQuestion.ID);
            }

            // 由Answer到Question
            using (var repo = RepositoryContext.GetRepository())
            {
                //SELECT * FROM [Answer] answer0_ WHERE answer0_.ID='029ECE8D-ACD5-461A-BE09-4099708494A7';
                var testAnswer = repo.GetByID<Answer>(answerID);

                //SELECT * FROM [Question] this_ WHERE this_.ID = '0F20A549-3E1E-499D-9ADF-06BBD472E0BE';
                var testQuestion = repo.GetAll<Question>(p => p.ID == testAnswer.Question.ID);

                Assert.AreEqual(testQuestion[0].ID, testAnswer.Question_ID);
            }
        }

        /// <summary>
        /// 立即加载的导航属性在兰姆达表达式中的测试
        /// </summary>
        [TestMethod]
        public void NavigationImmediatelyLoadTest()
        {
            Question1 question;
            Guid answerID;
            // 添加数据记录
            using (var repo = RepositoryContext.GetRepository())
            {
                question = new Question1() {Name = "question1123", CreatedOn = DateTime.Now, UpdatedOn = DateTime.Now};


                var addAnswer1 = new Answer1()
                                     {
                                         Name = "answer1",
                                         CreatedOn = DateTime.Now,
                                         UpdatedOn = DateTime.Now,
                                         Question = question
                                     };
                var addAnswer2 = new Answer1()
                                     {
                                         Name = "answer2",
                                         CreatedOn = DateTime.Now,
                                         UpdatedOn = DateTime.Now,
                                         Question = question
                                     };
                answerID = addAnswer1.ID;

                question.Answers.Add(addAnswer1);
                question.Answers.Add(addAnswer2);

                repo.Add(question);

                repo.SaveChanges();
            }

            // 由Question到Answer
            using (var repo = RepositoryContext.GetRepository())
            {
                // SELECT * FROM [Question] question0_ WHERE question0_.ID = '806B63F5-6B89-47E8-B47B-E05DC6B67AF3';
                // SELECT answers0_.Question_ID as Question8_1_, * FROM [Answer] answers0_ WHERE answers0_.Question_ID = '806B63F5-6B89-47E8-B47B-E05DC6B67AF3';
                var testQuestion = repo.GetByID<Question1>(question.ID);

                // 导航属性SQL测试                
                // SELECT * FROM [Answer] this_ WHERE this_.Question_ID = '806B63F5-6B89-47E8-B47B-E05DC6B67AF3';
                var answer =
                    repo.GetAll<Answer1>(p => p.Question_ID == testQuestion.Answers.FirstOrDefault().Question.ID);

                // 单元测试插入了两条answer数据
                Assert.AreEqual(answer[0].Question_ID, testQuestion.ID);
                Assert.AreEqual(answer[1].Question_ID, testQuestion.ID);                
            }

            // 由Answer到Question
            using (var repo = RepositoryContext.GetRepository())
            {
                // SELECT * FROM [Answer] answer0_ WHERE answer0_.ID='304438EC-30E1-4172-BA5B-25DF2F4D3019';
                // SELECT * FROM [Question] this_ WHERE this_.ID = '3EA33462-6E2F-4CF5-BD40-7B479010B572';
                var testAnswer = repo.GetByID<Answer1>(answerID);

                // 导航属性SQL测试  
                // SELECT answers0_.Question_ID as Question8_1_, * FROM [Answer] answers0_ WHERE answers0_.Question_ID='3EA33462-6E2F-4CF5-BD40-7B479010B572';
                var testQuestion = repo.GetAll<Question1>(p => p.ID == testAnswer.Question.ID);

                Assert.AreEqual(testQuestion[0].ID, testAnswer.Question_ID);
            }
        }

        /// <summary>
        /// 一对多，一端懒加载测试
        /// 默认情况下为懒加载情况SQL语句如下所示。
        /// </summary>
        [TestMethod]
        public void OneSideLazyLoadTest()
        {
            Question question;
            Guid answerID;
            // 添加数据记录
            using (var repo = RepositoryContext.GetRepository())
            {
                question = new Question() { Name = "question1123", CreatedOn = DateTime.Now, UpdatedOn = DateTime.Now };


                var addAnswer1 = new Answer()
                {
                    Name = "answer1",
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now,
                    Question = question
                };
                var addAnswer2 = new Answer()
                {
                    Name = "answer2",
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now,
                    Question = question
                };
                answerID = addAnswer1.ID;

                question.Answers.Add(addAnswer1);
                question.Answers.Add(addAnswer2);

                repo.Add(question);

                repo.SaveChanges();
            }

            // 由Answer到Question
            using (var repo = RepositoryContext.GetRepository())
            {
                //SELECT * FROM [Answer] answer0_ WHERE answer0_.ID='029ECE8D-ACD5-461A-BE09-4099708494A7';
                var testAnswer = repo.GetByID<Answer>(answerID);
                //SELECT * FROM [Question] this_ WHERE this_.ID = '0F20A549-3E1E-499D-9ADF-06BBD472E0BE';
                var tQuestion = testAnswer.Question;

                Assert.AreEqual(tQuestion.ID, question.ID);
                //SELECT * FROM [Question] this_ WHERE this_.ID = '0F20A549-3E1E-499D-9ADF-06BBD472E0BE';
                var testQuestion = repo.GetAll<Question>(p => p.ID == testAnswer.Question.ID);

                Assert.AreEqual(testQuestion[0].ID, testAnswer.Question_ID);
            }
        }

        /// <summary>
        /// 一对多，一端立即加载测试
        /// lazy = false情况SQL语句如下所示。
        /// </summary>
        [TestMethod]
        public void OneSideImmediatelyLoadTest()
        {
            Question2 question;
            Guid answerID;
            // 添加数据记录
            using (var repo = RepositoryContext.GetRepository())
            {
                question = new Question2() { Name = "question1123", CreatedOn = DateTime.Now, UpdatedOn = DateTime.Now };


                var addAnswer1 = new Answer2()
                {
                    Name = "answer1",
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now,
                    Question = question
                };
                var addAnswer2 = new Answer2()
                {
                    Name = "answer2",
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now,
                    Question = question
                };
                answerID = addAnswer1.ID;

                question.Answers.Add(addAnswer1);
                question.Answers.Add(addAnswer2);

                repo.Add(question);

                repo.SaveChanges();
            }

            // 由Answer到Question
            using (var repo = RepositoryContext.GetRepository())
            {
                //SELECT * FROM [Answer] answer0_ WHERE answer0_.ID='029ECE8D-ACD5-461A-BE09-4099708494A7';
                //SELECT * FROM [Question] this_ WHERE this_.ID = '0F20A549-3E1E-499D-9ADF-06BBD472E0BE';
                var testAnswer = repo.GetByID<Answer2>(answerID);
                
                var tQuestion = testAnswer.Question;

                Assert.AreEqual(tQuestion.ID, question.ID);
                //SELECT * FROM [Question] this_ WHERE this_.ID = '0F20A549-3E1E-499D-9ADF-06BBD472E0BE';
                var testQuestion = repo.GetAll<Question2>(p => p.ID == testAnswer.Question.ID);

                Assert.AreEqual(testQuestion[0].ID, testAnswer.Question_ID);
            }
        }

        /// <summary>
        /// 一对多，一端no-proxy加载测试
        /// lazy="no-proxy"        
        /// </summary>
        [TestMethod]
        public void OneSideEagerlyLoadTest()
        {
            Question question;
            Guid answerID;
            // 添加数据记录
            using (var repo = RepositoryContext.GetRepository())
            {
                question = new Question() { Name = "question1123", CreatedOn = DateTime.Now, UpdatedOn = DateTime.Now };


                var addAnswer1 = new Answer()
                {
                    Name = "answer1",
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now,
                    Question = question
                };
                var addAnswer2 = new Answer()
                {
                    Name = "answer2",
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now,
                    Question = question
                };
                answerID = addAnswer1.ID;

                question.Answers.Add(addAnswer1);
                question.Answers.Add(addAnswer2);

                repo.Add(question);

                repo.SaveChanges();
            }

            using (var repo = RepositoryContext.GetRepository())
            {
                var testAnswer = repo.GetByID<Answer>(answerID);

                testAnswer.XXX().Test();
            }
        }
    }
}
