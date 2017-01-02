using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication1.Models;
using WebApplication3.Models;

namespace WebApplication1.Controllers
{
    public class AnnouncementController : ApiController
    {
        public IEnumerable<Table> Get()
        {
            using (MessagesDatabaseEntities entities = new MessagesDatabaseEntities())
            {
                Messages msg = new Messages();
                return (msg.GetMessages(entities).OrderByDescending(e => e.Id).ToList().Take(10));
            }
        }


        public HttpResponseMessage Get(int id)
        {
            using (MessagesDatabaseEntities entities = new MessagesDatabaseEntities())
            {
                var entity = entities.Table.FirstOrDefault(e => e.Id == id);
                if (entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                        "Message with Id " + id.ToString() + " not found");
                }
            }
        }

        public HttpResponseMessage Post([FromBody] Table table)
        {
            try
            {
                using (MessagesDatabaseEntities entities = new MessagesDatabaseEntities())
                {
                    entities.Table.Add(table);
                    entities.SaveChanges();
                    var info = Request.CreateResponse(HttpStatusCode.Created, table);
                    info.Headers.Location = new Uri(Request.RequestUri +
                        table.Id.ToString());

                    return info;
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        public HttpResponseMessage Put(int id, [FromBody]Table table)
        {
            try
            {
                using (MessagesDatabaseEntities entities = new MessagesDatabaseEntities())
                {
                    var entity = entities.Table.FirstOrDefault(e => e.Id == id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                            "Message with Id " + id.ToString() + " not found to update");
                    }
                    else
                    {
                        entity.Header = table.Header;
                        entity.Body = table.Body;
                        entity.Date = table.Date;

                        entities.SaveChanges();

                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        public HttpResponseMessage Delete(int id)
        {
            try
            {
                using (MessagesDatabaseEntities entities = new MessagesDatabaseEntities())
                {
                    var entity = entities.Table.FirstOrDefault(e => e.Id == id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                            "Message with Id = " + id.ToString() + " not found to delete");
                    }
                    else
                    {
                        entities.Table.Remove(entity);
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}