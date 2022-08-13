 using haber1.Context;
using haber1.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using PagedList;
using PagedList.Mvc;
using System.Security.Claims;

namespace haber1.Controllers
{

    [Route("api/v1/[controller]")]
    public class HaberlerController : Controller
    {
        HaberContext context;

        public HaberlerController()
        {
            context = new HaberContext();
        }

        public class sonuc<T>
        {
            public bool success { get; set; }
            public string mesaj { get; set; }
            public List<T> data { get; set; }
        }
        public class sonuc
        {
            public bool success { get; set; }
            public int variables { get; set; }
            public string mesaj { get; set; }
        }
        private User getLoginUser()
        {
            string login_user_id = ((ClaimsIdentity)User.Identity).FindFirst("userid").Value;
            return context.Users.Find(login_user_id);
        }

        
        [HttpGet("[action]")]
        [AllowAnonymous]
        public IActionResult GetAll()
        {

            var haberler = context.News.ToList();
            return Ok(haberler);

        }
        [HttpGet("[action]")]
        [AllowAnonymous]
        public IActionResult GetUser()
        {

            var userList = context.Users.ToList();
            return Ok(userList);

        }



        [AllowAnonymous]
        [HttpGet("[action]/{newsid}")]
        public IActionResult GetNewById(int newsid)
        {

            var haberler = context.News.Where(x => x.NewsId == newsid).ToList();
            return Ok(haberler);

        }

        [Authorize]
        [HttpDelete("[action]/{id}")]
        public IActionResult HaberSilme(int id)
        {
            User login_usr = getLoginUser();
            var snc = new sonuc();
            snc.mesaj = "";
            if (login_usr.UserType == 2)
            {
                snc.mesaj = "Yetkiniz yoktur.";
                snc.success = false;
                return Ok(snc.mesaj);
            }
            else
            {
                try
                {
                    var haber = context.News.Find(id);
                    if (haber.UserId == login_usr.UserId)
                    {

                        if (haber == null)
                        {
                            return Ok("Bu haber zaten yok");
                        }
                        context.News.Remove(haber);
                        context.SaveChanges();
                        snc.mesaj = "Haber silindi";
                        return Ok(snc.mesaj);

                    }
                    else if (login_usr.UserType == 0)
                    {
                        context.News.Remove(haber);
                        context.SaveChanges();
                        snc.mesaj = "Haber silindi";
                        return Ok(snc.mesaj);
                    }
                    else
                    {
                        snc.mesaj = "Yetkiniz yoktur.";
                        snc.success = false;
                        return Ok(snc.mesaj);
                    }

                }
                catch (Exception ex)
                {
                    snc.mesaj = "Hata aldınız...";
                    return Ok(snc.mesaj + ex.Message);

                }
            }

        }

        [HttpPost("[action]")]
        [Authorize("1")]
        public IActionResult HaberEkleme(News haber)
        {
            User login_usr = getLoginUser();
            var snc = new sonuc();
            snc.mesaj = "";
            if (login_usr.UserType < 2)
            {
                try
                {
                    haber.DatePosted = DateTime.Now;
                    context.News.Add(haber);
                    context.SaveChanges();
                    snc.mesaj = "Haber eklendi" + login_usr.Username + "bilginize.";
                    snc.success = true;
                    return Ok(snc.mesaj);

                }
                catch (Exception ex)
                {
                    snc.mesaj = "Haber eklenemedi.Hata :";
                    return Ok(snc.mesaj + ex.Message);
                }
            }
            else
            {
                snc.mesaj = "Sizin böyle bir yetkiniz ne yazık ki bulunmamaktadır...";
                return Ok(snc.mesaj);
            }

        }

        [HttpPut("[action]")]
        [Authorize("1")]
        public ActionResult HaberGuncelleme(News haber)
        {
            User login_usr = getLoginUser();
            sonuc snc = new sonuc();
            snc.success = false;
            if (login_usr.UserType == 2)
            {
                snc.mesaj = "Yetkiniz yoktur.";
                snc.success = false;
                return Ok(snc.mesaj);
            }
            else
            {
                if (login_usr.UserId == haber.UserId)
                {

                    try
                    {
                        haber.DatePosted = DateTime.Now;
                        context.News.Update(haber);
                        context.SaveChanges();
                        snc.success = true;
                        snc.mesaj = "Haber güncellendi";
                        return Ok(snc.mesaj);
                    }
                    catch (Exception ex)
                    {
                        snc.mesaj = "Hata: " + ex.ToString(); ;
                        return Ok(snc);
                    }


                }
                else if (login_usr.UserType == 0)
                {
                    haber.DatePosted = DateTime.Now;
                    context.News.Update(haber);
                    context.SaveChanges();
                    snc.success = true;
                    snc.mesaj = "Haber güncellendi";
                    return Ok(snc.mesaj);
                }
                else
                {
                    snc.mesaj = "Haber güncellenemedi";
                    return Ok(snc.mesaj);
                }
            }


        }
       
        [HttpGet("[action]")]
        [AllowAnonymous]
        public IActionResult GetAllLastDokuz()
        {
            var haberler = context.News.OrderByDescending(x => x.NewsId).Take(9).ToList();
            return Ok(haberler);
        }

        [HttpGet("[action]")]
        [Authorize]
        public IActionResult GetAllLastThree()
        {
            var haberler = context.News.OrderByDescending(x => x.NewsId).Take(3).ToList();
            return Ok(haberler);
        }


        [HttpPost("[action]")]
        public IActionResult UserEkleme([FromBody] User user)
        {
            sonuc snc = new sonuc();
            snc.success = false;

            try
            {
                context.Users.Add(user);
                context.SaveChanges();
                snc.mesaj = "Kullanıcı eklendi.";
                snc.success = true;
            
                return Ok(snc.mesaj);
            }

                
            catch (Exception ex)
            {
                snc.mesaj= "Kullanıcı eklenemedi. Hata :";
                snc.success= false;
                return Ok(snc.mesaj + ex.Message);
            }

            
            

            
           
        }

        [HttpDelete("[action]/{id}")]
        [Authorize("0")]
        public IActionResult UserCikartma(int id)
        {
            User login_usr = getLoginUser();
            sonuc snc = new sonuc();
            snc.success = false;
            if (login_usr.UserType == 0)
            {
                try
                {
                    var yazar = context.Users.Find(id);
                    if (yazar == null)
                    {
                        return Ok("Bu yazar zaten yok");
                    }
                    context.Users.Remove(yazar);
                    context.SaveChanges();
                    snc.success = true;
                    snc.mesaj = "Kullanıcı silindi.";
                    return Ok(snc.mesaj);

                }
                catch (Exception ex)
                {
                    snc.success = false;
                    return Ok("Yazar eklenemedi. Hata: " + ex.Message);

                }
            }
            else
            {
                snc.success = false;
                snc.mesaj = "Yetkiniz yok...";
                return Ok(snc.mesaj);
            }
        }
        
        [HttpPut("[action]")]
        [AllowAnonymous]
        public IActionResult UserUpdate(User user)
        {

            User login_usr = getLoginUser();
            sonuc snc = new sonuc();
            snc.success = false;
            if(login_usr.UserType!= 0)
            {
                try
                {
                //var UserEski = context.Users.Find(user.UserId);
                //UserEski= user;
                    context.Users.Update(user);
                    context.SaveChanges();
                    snc.mesaj = "Kullanıcı güncellendi.";
                    snc.success = true;
                    return Ok(snc.mesaj);
                }
                catch (Exception ex)
                {
                    snc.mesaj = "Kullanıcı güncellenemedi:";
                    snc.success = false;
                    return Ok(snc.mesaj + login_usr.Username + ex.ToString());
                }
            }
            else
            {
                snc.mesaj = "Kullanıcı güncellenemedi";
                return Ok(snc.mesaj);
            }
        }
        
        [HttpGet("[action]/{newPage}/{itemsPerPage}")]
        [AllowAnonymous]
        public IActionResult GoToPage(int newPage, int itemsPerPage)
        {
            var snc = new sonuc<News>();
            //if totali geçiyor mu
            var result = context.News.OrderByDescending(x=>x.NewsId).Skip((newPage-1)*itemsPerPage).Take(itemsPerPage);
            snc.data = result.ToList();
            snc.mesaj = context.News.Count().ToString();
            snc.success = true;
            return Ok(snc);
          
        }
        
        [HttpGet("[action]/{category_id}")]
        [AllowAnonymous]
        public IActionResult GetByCategory(int category_id)
        {
            var haber = context.News.Where(x=> x.CategoryId==category_id);

            return Ok(haber);
        }
        
        [HttpGet("[action]")]
        [AllowAnonymous]
        public IActionResult getkategs()
        {
            var kat_id =context.NewsCategories.ToList();


            return Ok(kat_id);
        }


    }
}
