using InterTwitter.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace InterTwitter.Services
{
    public class MockService : IMockService
    {
        private readonly TaskCompletionSource<bool> _initCompletionSource = new TaskCompletionSource<bool>();

        private IList<UserModel> _users;
        private IList<TweetModel> _tweets;
        private IList<Bookmark> _bookmarks;
        private IList<LikeModel> _likes;
        private IList<HashtagModel> _hashtags;
        private IList<BlockModel> _blackList;
        private IList<MuteModel> _muteList;

        private Dictionary<Type, object> _base;

        public MockService()
        {
            Task.Run(InitMocksAsync);
        }

        #region -- IMockService implementation --

        public async Task<int> AddAsync<T>(T entity)
            where T : IEntityBase, new()
        {
            await _initCompletionSource.Task;
            int id = 1;

            if (GetBase<T>().Count > 0)
            {
                id = GetBase<T>().Max(x => x.Id) + 1;
                entity.Id = id;
            }
            else
            {
                entity.Id = 1;
            }

            GetBase<T>().Add(entity);

            return id;
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>()
            where T : IEntityBase, new()
        {
            await _initCompletionSource.Task;

            return GetBase<T>();
        }

        public async Task<T> GetByIdAsync<T>(int id)
            where T : IEntityBase, new()
        {
            await _initCompletionSource.Task;

            return GetBase<T>().FirstOrDefault(x => x.Id == id);
        }

        public async Task<bool> RemoveAsync<T>(T entity)
            where T : IEntityBase, new()
        {
            await _initCompletionSource.Task;

            var entityDelete = GetBase<T>().FirstOrDefault(x => x.Id == entity.Id);

            return GetBase<T>().Remove(entityDelete);
        }

        public async Task<int> RemoveAllAsync<T>(Predicate<T> predicate)
            where T : IEntityBase, new()
        {
            await _initCompletionSource.Task;

            return GetBase<T>().RemoveAll(predicate);
        }

        public async Task<T> UpdateAsync<T>(T entity)
            where T : IEntityBase, new()
        {
            await _initCompletionSource.Task;

            var entityUpdate = GetBase<T>().FirstOrDefault(x => x.Id == entity.Id);
            entityUpdate = entity;

            return entityUpdate;
        }

        public async Task<T> FindAsync<T>(Func<T, bool> expression)
            where T : IEntityBase, new()
        {
            await _initCompletionSource.Task;

            return GetBase<T>().FirstOrDefault<T>(expression);
        }

        public async Task<bool> AnyAsync<T>(Func<T, bool> expression)
            where T : IEntityBase, new()
        {
            await _initCompletionSource.Task;

            return GetBase<T>().Any<T>(expression);
        }

        public async Task<IEnumerable<T>> GetAsync<T>(Func<T, bool> expression)
            where T : IEntityBase, new()
        {
            await _initCompletionSource.Task;

            return GetBase<T>().Where<T>(expression);
        }

        #endregion

        #region -- Private helpers --

        private List<T> GetBase<T>()
        {
            return (List<T>)_base[typeof(T)];
        }

        private async Task InitMocksAsync()
        {
            _base = new Dictionary<Type, object>();

            await Task.WhenAll(
                InitUsersAsync(),
                InitTweetsAsync(),
                InitBookmarks(),
                InitLikes(),
                InitHashtags(),
                InitBlackList(),
                InitMuteList());

            _initCompletionSource.TrySetResult(true);
        }

        private Task InitBlackList() => Task.Run(() =>
        {
            _blackList = new List<BlockModel>();

            _base.Add(typeof(BlockModel), _blackList);
        });

        private Task InitMuteList() => Task.Run(() =>
        {
            _muteList = new List<MuteModel>();

            _base.Add(typeof(MuteModel), _muteList);
        });

        private Task InitHashtags() => Task.Run(() =>
        {
            _hashtags = new List<HashtagModel>
            {
                new HashtagModel()
                {
                    Id = 1,
                    Text = "#blockchain",
                    TweetsCount = 2,
                },
                new HashtagModel()
                {
                    Id = 2,
                    Text = "#AMAs",
                    TweetsCount = 4,
                },
                new HashtagModel()
                {
                    Id = 3,
                    Text = "#NoNuanceNovember",
                    TweetsCount = 3,
                },
                new HashtagModel()
                {
                    Id = 4,
                    Text = "#coffeeTime",
                    TweetsCount = 2,
                },
                new HashtagModel()
                {
                    Id = 5,
                    Text = "#teaTime",
                    TweetsCount = 1,
                },
                new HashtagModel()
                {
                    Id = 6,
                    Text = "#workout",
                    TweetsCount = 1,
                },
                new HashtagModel()
                {
                    Id = 7,
                    Text = "#cats",
                    TweetsCount = 1,
                },
            };

            _base.Add(typeof(HashtagModel), _hashtags);
        });

        private Task InitUsersAsync() => Task.Run(() =>
        {
            _users = new List<UserModel>
            {
                new UserModel
                {
                    Id = 1,
                    Name = "Bill Gates",
                    Email = "aaa@aaa.aaa",
                    Password = "1234567A",
                    AvatarPath = "https://cdn.allfamous.org/people/avatars/bill-gates-zdrr-allfamous.org.jpg",
                    BackgroundUserImagePath = "https://cdn.pixabay.com/photo/2016/11/29/03/44/animal-1867125__340.jpg",
                },
                new UserModel
                {
                    Id = 2,
                    Name = "Kate White",
                    Email = "bbb@bbb.bbb",
                    Password = "1234567A",
                    AvatarPath = "https://www.iso.org/files/live/sites/isoorg/files/news/News_archive/2021/03/Ref2639/Ref2639.jpg/thumbnails/300x300",
                    BackgroundUserImagePath = "https://cdn.pixabay.com/photo/2021/02/02/17/13/trees-5974614__340.jpg",
                },
                new UserModel
                {
                    Id = 3,
                    Name = "Sam Smith",
                    Email = "ccc@ccc.ccc",
                    Password = "1234567A",
                    AvatarPath = "https://i.ebayimg.com/images/g/6EIAAOSwJHlfnm3a/s-l300.jpg",
                    BackgroundUserImagePath = "https://cdn.pixabay.com/photo/2021/12/06/16/38/cedar-6850925__340.jpg",
                },
                new UserModel
                {
                    Id = 4,
                    Name = "Steve Jobs",
                    Email = "ddd@ddd.ddd",
                    Password = "1234567A",
                    AvatarPath = "https://www.acumarketing.com/wp-content/uploads/2011/08/steve-jobs.jpg",
                    BackgroundUserImagePath = "https://yapx.ru/viral/PMYaG",
                },
                new UserModel
                {
                    Id = 5,
                    Name = "Elon musk",
                    Email = "eee@eee.eee",
                    Password = "1234567A",
                    AvatarPath = "https://file.liga.net/images/general/2021/09/20/thumbnail-20210920123323-9397.jpg?v=1632132620",
                    BackgroundUserImagePath = "https://cdn.pixabay.com/photo/2017/02/25/18/48/rowan-2098486__340.jpg",
                },
                new UserModel
                {
                    Id = 6,
                    Name = "Keano Reaves",
                    Email = "fff@fff.fff",
                    Password = "1234567A",
                    AvatarPath = "https://www.biography.com/.image/ar_1:1%2Cc_fill%2Ccs_srgb%2Cg_face%2Cq_auto:good%2Cw_300/MTE5NTU2MzE2MzU1NzI0ODEx/keanu-reeves-9454211-1-402.jpg",
                    BackgroundUserImagePath = "https://cdn.pixabay.com/photo/2019/12/18/19/19/christmas-4704703__340.jpg",
                },
                new UserModel
                {
                    Id = 7,
                    Name = "Roderick Marvin",
                    Email = "ggg@ggg.ggg",
                    Password = "1234567A",
                    AvatarPath = "https://cdn.pixabay.com/photo/2015/01/27/09/58/man-613601_960_720.jpg",
                    BackgroundUserImagePath = "https://cdn.pixabay.com/photo/2021/10/26/12/34/christmas-6743572__340.jpg",
                },
                new UserModel
                {
                    Id = 8,
                    Name = "Clinton Gleichner",
                    Email = "hhh@hhh.hhh",
                    Password = "1234567A",
                    AvatarPath = "https://media.istockphoto.com/photos/mid-adult-businessman-at-work-picture-id1150504096",
                    BackgroundUserImagePath = "https://cdn.pixabay.com/photo/2020/03/05/17/07/montserrat-4904951__340.jpg",
                },
                new UserModel
                {
                    Id = 9,
                    Name = "Victor Dickinson",
                    Email = "iii@iii.iii",
                    Password = "1234567A",
                    AvatarPath = "https://cdn.pixabay.com/photo/2015/01/08/18/29/entrepreneur-593358_960_720.jpg",
                    BackgroundUserImagePath = "https://cdn.pixabay.com/photo/2021/12/18/17/11/flower-6879399__340.jpg",
                },
                new UserModel
                {
                    Id = 10,
                    Name = "Dave Glover",
                    Email = "jjj@jjj.jjj",
                    Password = "1234567A",
                    AvatarPath = "https://cdn.pixabay.com/photo/2017/08/01/01/33/beanie-2562646_960_720.jpg",
                    BackgroundUserImagePath = "https://cdn.pixabay.com/photo/2019/11/25/06/41/market-4651117__340.jpg",
                },
            };

            _base.Add(typeof(UserModel), _users);
        });

        private Task InitTweetsAsync() => Task.Run(() =>
        {
            var culture = CultureInfo.GetCultureInfo("ru-RU");

            _tweets = new List<TweetModel>
            {
                new TweetModel
                {
                    Id = 1,
                    UserId = 1,
                    Media = Enums.EAttachedMediaType.Photos,
                    Text = "01 um quisquam eius sed odit fugiat iusto fuga praesentium optio, eaque rerum! Provident similique accusantium nemo autem. Veritatisobcaecati tenetur iure eius earum ut molestias architecto voluptate aliquam",
                    MediaPaths = new List<string>
                    {
                        "https://oboi-lux.com.ua/23613-home_default/fotooboi-s-prirodoj.jpg",
                        "https://fotooboimarket.com.ua/11134-home_default/priroda.jpg",
                        "https://oboi-lux.com.ua/23595-home_default/fotooboi-s-prirodoj.jpg",
                        "http://intpicture.com/wp-content/uploads/2011/07/Nature-64-034-Copy-300x300.jpg",
                    },
                    CreationTime = DateTime.Parse("01.01.2020 12:12:12", culture),
                },
                new TweetModel
                {
                    Id = 2,
                    UserId = 3,
                    Text = "#NoNuanceNovember um quisquam eius #AMAs sed odit fugiat iusto fuga #blockchain praesentium optio, eaque rerum! Provident similique accusantium nemo autem. Veritatisobcaecati tenetur iure eius earum ut molestias architecto voluptate aliquam",
                    Media = Enums.EAttachedMediaType.Video,
                    MediaPaths = new List<string>
                    {
                       "https://www.youtube.com/embed/_hGuLM4Y-xM",
                    },
                    CreationTime = DateTime.Parse("01.03.2020 13:12:12", culture),
                },
                new TweetModel
                {
                    Id = 3,
                    UserId = 4,
                    Text = "#AMAs Only #NoNuanceNovember text #coffeeTime",
                    Media = Enums.EAttachedMediaType.None,
                    CreationTime = DateTime.Parse("01.04.2020 15:12:12", culture),
                },
                new TweetModel
                {
                    Id = 4,
                    UserId = 5,
                    Text = " #teaTime Hi #blockchain there! ",
                    Media = Enums.EAttachedMediaType.Photos,
                    CreationTime = DateTime.Parse("01.05.2021 12:00:12", culture),
                    MediaPaths = new List<string>
                    {
                       "https://akm-img-a-in.tosshub.com/indiatoday/images/story/202110/Elon-Musk_1200x768.png?WgPbwF44wHXitAAL7xu8BNhZXQXIPBbV&size=770:433",
                       "https://i.pinimg.com/550x/23/b4/b1/23b4b13e9019667ca68d0897e154a755.jpg",
                    },
                },
                new TweetModel
                {
                    Id = 5,
                    UserId = 5,
                    Media = Enums.EAttachedMediaType.Photos,
                    CreationTime = DateTime.Parse("01.11.2020 12:00:12", culture),
                    MediaPaths = new List<string>
                    {
                       "https://i.pinimg.com/474x/f7/f7/73/f7f7733f9e3409f0ef433f3074525790.jpg",
                    },
                },
                new TweetModel
                {
                    Id = 6,
                    UserId = 6,
                    Text = "#workout #NoNuanceNovember #workout #AMAs #workouthello HI#workout there#workout!",
                    Media = Enums.EAttachedMediaType.Gif,
                    MediaPaths = new List<string>
                    {
                        "https://javasea.ru/uploads/posts/2013-12/1387635240_matrica.gif",
                    },
                    CreationTime = DateTime.Parse("01.01.2021 12:12:12", culture),
                },
                new TweetModel
                {
                    Id = 7,
                    UserId = 2,
                    Text = "#AMAs onsequuntur #cats voluptatum laborum #coffeeTime numquam blanditiis harum quisquam eius sed odit fugiat iusto fuga praesentium optio, eaque rerum! Provident similique accusantium nemo autem. Veritatisobcaecati tenetur iure eius earum ut molestias architecto voluptate aliquam",
                    Media = Enums.EAttachedMediaType.Gif,
                    MediaPaths = new List<string>
                    {
                        "https://thumbs.gfycat.com/PaltryWickedCrayfish-max-1mb.gif",
                    },
                    CreationTime = DateTime.Now,
                },
                new TweetModel
                {
                    Id = 8,
                    UserId = 2,
                    Text = "#amas masd",
                    Media = Enums.EAttachedMediaType.Gif,
                    MediaPaths = new List<string>
                    {
                        "https://thumbs.gfycat.com/PaltryWickedCrayfish-max-1mb.gif",
                    },
                    CreationTime = DateTime.Parse("01.02.2021 13:12:12", culture),
                },
                new TweetModel
                {
                    Id = 9,
                    UserId = 3,
                    //Text = "#AMAs masd # masda as ama",
                    Media = Enums.EAttachedMediaType.Gif,
                    MediaPaths = new List<string>
                    {
                        "https://thumbs.gfycat.com/PaltryWickedCrayfish-max-1mb.gif",
                    },
                    CreationTime = DateTime.Parse("01.02.2021 12:12:12", culture),
                },
                new TweetModel
                {
                    Id = 10,
                    UserId = 10,
                    Text = "Qui accusantium fuga. Repudiandae est et in aliquid voluptatem excepturi repellat. Eligendi voluptates quo hic ut sed totam. Voluptas voluptas qui ad voluptatem. Quas magni dolorem.",
                    Media = Enums.EAttachedMediaType.Photos,
                    MediaPaths = new List<string>
                    {
                        "https://cdn.pixabay.com/photo/2021/12/09/20/58/christmas-cookies-6859116__340.jpg",
                        "https://cdn.pixabay.com/photo/2021/12/04/15/54/santa-claus-6845491__340.jpg",
                        "https://cdn.pixabay.com/photo/2016/12/25/19/41/france-1930733__340.jpg",
                        "https://cdn.pixabay.com/photo/2021/12/20/15/01/christmas-tree-6883263__340.jpg",
                        "https://cdn.pixabay.com/photo/2021/10/26/12/34/christmas-6743572__340.jpg",
                        "https://cdn.pixabay.com/photo/2021/01/03/13/57/gingerbread-5884607__340.jpg",
                    },
                    CreationTime = DateTime.Parse("02.12.2021 23:39:59", culture),
                },
                new TweetModel
                {
                    Id = 11,
                    UserId = 9,
                    Text = "Occaecat nostrud cillum ipsum enim eiusmod ut eiusmod amet exercitation. Irure quis incididunt mollit adipisicing ipsum dolor ex laborum voluptate. Non cupidatat aute enim in est commodo aliquip eu consequat nisi qui et. Velit fugiat nulla ullamco quis. Minim excepteur aliquip sunt dolor deserunt sint amet.",
                    Media = Enums.EAttachedMediaType.Photos,
                    MediaPaths = new List<string>
                    {
                        "https://thumbs.dreamstime.com/z/footbridge-over-pond-18353918.jpg",
                    },
                    CreationTime = DateTime.Parse("12.11.2021 18:05:43", culture),
                },
                new TweetModel
                {
                    Id = 12,
                    UserId = 8,
                    Text = "Enim anim in ea aliqua laborum laboris. Duis officia sunt aliqua consequat cupidatat nisi excepteur nulla. Eiusmod officia fugiat elit ex minim ea laboris amet occaecat et ea. Et labore consequat commodo elit consectetur incididunt anim anim. Nisi consectetur elit incididunt ex duis eiusmod. Consectetur excepteur Lorem ullamco do incididunt dolor reprehenderit aute in. Duis officia veniam nisi duis adipisicing magna officia id.",
                    Media = Enums.EAttachedMediaType.Photos,
                    MediaPaths = new List<string>
                    {
                        "https://thumbs.dreamstime.com/z/fruit-tart-food-10443119.jpg",
                    },
                    CreationTime = DateTime.Parse("22.07.2021 05:46:23", culture),
                },
                new TweetModel
                {
                    Id = 13,
                    UserId = 7,
                    Text = "Quis magna irure nisi ad esse amet ullamco voluptate laboris. Cupidatat aute do commodo aliquip in aliqua aute dolore ipsum amet mollit non. Do consequat ut cupidatat nulla nostrud culpa nulla. Veniam ut ea Lorem veniam. Officia amet voluptate id voluptate Lorem cupidatat ullamco voluptate commodo commodo cupidatat id velit id. Veniam magna minim cupidatat culpa eu consequat sint irure cillum consequat minim officia eiusmod.",
                    Media = Enums.EAttachedMediaType.Gif,
                    MediaPaths = new List<string>
                    {
                        "https://c.tenor.com/uIorgaD4_cUAAAAd/fake-news-point.gif",
                    },
                    CreationTime = DateTime.Parse("01.04.2021 21:47:38", culture),
                },
                new TweetModel
                {
                    Id = 14,
                    UserId = 6,
                    Text = "Magna ullamco laboris adipisicing esse culpa culpa laboris in esse adipisicing nostrud aute pariatur. Minim do dolor ut nisi adipisicing. Laborum laboris tempor ut reprehenderit labore anim officia nostrud magna eiusmod elit. Elit velit ex veniam fugiat est mollit cupidatat est laboris dolore deserunt eiusmod magna.",
                    Media = Enums.EAttachedMediaType.Gif,
                    MediaPaths = new List<string>
                    {
                        "https://c.tenor.com/FX5hQwWOgGgAAAAd/get-fake-get-real.gif",
                    },
                    CreationTime = DateTime.Parse("04.02.2021 21:48:56", culture),
                },
                new TweetModel
                {
                    Id = 15,
                    UserId = 5,
                    Text = "Velit ex consectetur velit laboris duis consequat nulla in qui occaecat nisi tempor irure. Anim deserunt Lorem ullamco deserunt reprehenderit. Enim ullamco minim ex Lorem cupidatat fugiat deserunt. Proident laboris aute esse quis anim. Excepteur dolor officia proident anim velit culpa magna incididunt aliqua enim nisi voluptate. Reprehenderit dolore voluptate enim voluptate sunt.",
                    Media = Enums.EAttachedMediaType.Gif,
                    MediaPaths = new List<string>
                    {
                        "https://c.tenor.com/46v9XAZEGhwAAAAC/the-addams-family-wednesday-addams.gif",
                    },
                    CreationTime = DateTime.Parse("14.03.2021 06:06:49", culture),
                },
                new TweetModel
                {
                    Id = 16,
                    UserId = 4,
                    Text = "Duis qui tempor aliqua commodo deserunt.",
                    Media = Enums.EAttachedMediaType.Gif,
                    MediaPaths = new List<string>
                    {
                        "https://c.tenor.com/ObIC7_jboZ0AAAAd/fake-pizza-lover-fake-pizza.gif",
                    },
                    CreationTime = DateTime.Parse("06.01.2021 21:21:12", culture),
                },
                new TweetModel
                {
                    Id = 17,
                    UserId = 3,
                    Text = "Id dolor pariatur nisi adipisicing elit minim ipsum aute laborum ipsum ad occaecat anim.",
                    Media = Enums.EAttachedMediaType.Gif,
                    MediaPaths = new List<string>
                    {
                        "https://c.tenor.com/RDywAE4gghYAAAAd/cousin-itt.gif",
                    },
                    CreationTime = DateTime.Parse("04.09.2021 05:15:40", culture),
                },
                new TweetModel
                {
                    Id = 18,
                    UserId = 2,
                    Text = "Fugiat exercitation laborum in ipsum amet in consectetur irure et amet aliquip nisi.",
                    Media = Enums.EAttachedMediaType.Gif,
                    MediaPaths = new List<string>
                    {
                        "https://c.tenor.com/K0fEKn9p4esAAAAd/morticia-addams-gomez-addams.gif",
                    },
                    CreationTime = DateTime.Parse("06.12.2020 07:04:57", culture),
                },
                new TweetModel
                {
                    Id = 19,
                    UserId = 1,
                    Text = "Do aliquip voluptate officia voluptate occaecat aute esse.",
                    Media = Enums.EAttachedMediaType.Gif,
                    MediaPaths = new List<string>
                    {
                        "https://c.tenor.com/t8eiWmNq0vAAAAAd/the-addams-family-morticia-addams.gif",
                    },
                    CreationTime = DateTime.Parse("15.12.2020 10:30:10", culture),
                },
                new TweetModel
                {
                    Id = 20,
                    UserId = 2,
                    Text = "Laboris velit pariatur dolor nostrud dolor pariatur laborum.",
                    Media = Enums.EAttachedMediaType.Video,
                    MediaPaths = new List<string>
                    {
                        "https://www.youtube.com/embed/CoLWsyLWoyI",
                    },
                    CreationTime = DateTime.Parse("21.11.2021 05:29:21", culture),
                },
                new TweetModel
                {
                    Id = 21,
                    UserId = 3,
                    Text = "Veniam ea do amet mollit ut est sit reprehenderit.",
                    Media = Enums.EAttachedMediaType.None,
                    CreationTime = DateTime.Parse("22.06.2021 05:34:36", culture),
                },
                new TweetModel
                {
                    Id = 22,
                    UserId = 4,
                    Text = "Duis occaecat aliqua dolor aute fugiat nostrud excepteur et commodo exercitation dolore enim consequat dolore.",
                    Media = Enums.EAttachedMediaType.None,
                    CreationTime = DateTime.Parse("22.08.2021 13:44:46", culture),
                },
                new TweetModel
                {
                    Id = 23,
                    UserId = 5,
                    Text = "Cillum adipisicing velit nostrud tempor amet dolore elit proident esse aute labore fugiat Lorem adipisicing.",
                    Media = Enums.EAttachedMediaType.None,
                    CreationTime = DateTime.Parse("03.02.2021 08:02:06", culture),
                },
                new TweetModel
                {
                    Id = 24,
                    UserId = 6,
                    Text = "Consequat excepteur amet velit quis ipsum velit excepteur ullamco sunt veniam minim est. Consectetur est reprehenderit duis ad velit. Elit aliqua anim anim minim ipsum aliquip nostrud pariatur. Mollit qui voluptate veniam incididunt.",
                    Media = Enums.EAttachedMediaType.Gif,
                    MediaPaths = new List<string>
                    {
                        "https://c.tenor.com/3dJ9UJR9lEUAAAAd/addamsfamilyvalues-addamsfamily.gif",
                    },
                    CreationTime = DateTime.Parse("27.10.2020 11:03:41", culture),
                },
                new TweetModel
                {
                    Id = 25,
                    UserId = 7,
                    Text = "Consectetur voluptate laborum proident mollit id ut sit nulla adipisicing incididunt esse. Enim nisi amet reprehenderit consectetur occaecat anim excepteur id ipsum aute duis ea aute ad. Ut fugiat ex labore duis nostrud consectetur. Officia minim laboris do do incididunt cupidatat occaecat reprehenderit.",
                    Media = Enums.EAttachedMediaType.Gif,
                    MediaPaths = new List<string>
                    {
                        "https://c.tenor.com/D2U3Ac1r2yQAAAAC/the-addams-family-wednesday-addams.gif",
                    },
                    CreationTime = DateTime.Parse("09.07.2021 10:27:08", culture),
                },
                new TweetModel
                {
                    Id = 26,
                    UserId = 8,
                    Text = "Aute pariatur irure ipsum ea elit laboris do culpa ex anim. Exercitation occaecat anim dolor ad tempor incididunt duis nulla. Id culpa qui quis occaecat laborum. Ut velit sint minim ex duis reprehenderit eiusmod. Lorem labore esse exercitation officia nulla. Nulla nisi veniam enim culpa occaecat. Duis eiusmod esse ea exercitation.",
                    Media = Enums.EAttachedMediaType.Gif,
                    MediaPaths = new List<string>
                    {
                        "https://c.tenor.com/gAt0bwNZce8AAAAd/humanveil-addams-family.gif",
                    },
                    CreationTime = DateTime.Parse("27.11.2020 08:17:12", culture),
                },
                new TweetModel
                {
                    Id = 27,
                    UserId = 9,
                    Text = "Consectetur sint aute proident anim occaecat cupidatat quis culpa eiusmod exercitation.",
                    Media = Enums.EAttachedMediaType.Gif,
                    MediaPaths = new List<string>
                    {
                        "https://c.tenor.com/Nar1dPh7J8sAAAAd/the-addams-family-wednesday-addams.gif",
                    },
                    CreationTime = DateTime.Parse("20.07.2021 20:15:53", culture),
                },
                new TweetModel
                {
                    Id = 28,
                    UserId = 10,
                    Text = "Incididunt adipisicing adipisicing nisi tempor magna cupidatat ad cillum exercitation non.",
                    Media = Enums.EAttachedMediaType.Photos,
                    MediaPaths = new List<string>
                    {
                        "https://cdn.pixabay.com/photo/2021/12/15/18/18/flowers-6873165__340.jpg",
                    },
                    CreationTime = DateTime.Parse("13.08.2021 17:31:29", culture),
                },
                new TweetModel
                {
                    Id = 29,
                    UserId = 9,
                    Media = Enums.EAttachedMediaType.Photos,
                    MediaPaths = new List<string>
                    {
                        "https://cdn.pixabay.com/photo/2016/12/19/18/21/snowflake-1918794__340.jpg",
                    },
                    CreationTime = DateTime.Parse("06.08.2021 21:30:01", culture),
                },
                new TweetModel
                {
                    Id = 30,
                    UserId = 8,
                    Text = "Dolor laboris enim Lorem voluptate consectetur officia velit ea incididunt aliqua. Pariatur incididunt reprehenderit qui labore quis magna non in culpa reprehenderit minim. Eiusmod in laborum sunt ut esse. Eiusmod sunt occaecat eu aliqua. In laborum dolore culpa occaecat laboris ipsum minim proident et duis. Cupidatat minim labore sint aliquip irure nisi adipisicing esse culpa magna laboris mollit. Reprehenderit ad dolore exercitation ex occaecat laboris reprehenderit dolor elit commodo id.",
                    Media = Enums.EAttachedMediaType.Photos,
                    MediaPaths = new List<string>
                    {
                        "https://cdn.pixabay.com/photo/2021/12/07/05/28/bird-6852282__340.jpg",
                    },
                    CreationTime = DateTime.Parse("05.04.2021 03:40:13", culture),
                },
                new TweetModel
                {
                    Id = 31,
                    UserId = 7,
                    Text = "Aliquip consectetur ut voluptate commodo laboris adipisicing sunt amet aliqua nisi labore do.",
                    Media = Enums.EAttachedMediaType.Photos,
                    MediaPaths = new List<string>
                    {
                        "https://cdn.pixabay.com/photo/2021/11/02/07/59/winter-6762640__340.jpg",
                    },
                    CreationTime = DateTime.Parse("24.06.2021 20:29:07", culture),
                },
                new TweetModel
                {
                    Id = 32,
                    UserId = 6,
                    Media = Enums.EAttachedMediaType.Photos,
                    MediaPaths = new List<string>
                    {
                        "https://cdn.pixabay.com/photo/2021/12/14/09/37/animal-6870176__340.jpg",
                    },
                    CreationTime = DateTime.Parse("06.12.2020 11:53:07", culture),
                },
                new TweetModel
                {
                    Id = 33,
                    UserId = 5,
                    Text = "Non ullamco ipsum exercitation Lorem laboris exercitation ullamco elit elit non culpa in incididunt id.",
                    Media = Enums.EAttachedMediaType.Photos,
                    MediaPaths = new List<string>
                    {
                        "https://cdn.pixabay.com/photo/2021/12/13/14/57/trees-6868446__340.jpg",
                    },
                    CreationTime = DateTime.Parse("10.12.2020 12:44:03", culture),
                },
            };

            _base.Add(typeof(TweetModel), _tweets);
        });

        private Task InitLikes() => Task.Run(() =>
        {
            var culture = CultureInfo.GetCultureInfo("ru-RU");

            _likes = new List<LikeModel>
            {
                new LikeModel
                {
                    Id = 1,
                    UserId = 1,
                    TweetId = 1,
                    Notification = true,
                    CreationTime = DateTime.Parse("02.12.2021 12:12:12", culture),
                },
                new LikeModel
                {
                    Id = 2,
                    UserId = 1,
                    TweetId = 2,
                    Notification = true,
                    CreationTime = DateTime.Parse("01.12.2021 12:12:12", culture),
                },
                new LikeModel
                {
                    Id = 3,
                    UserId = 1,
                    TweetId = 3,
                    Notification = true,
                    CreationTime = DateTime.Parse("02.12.2021 12:12:12", culture),
                },
                new LikeModel
                {
                    Id = 4,
                    UserId = 1,
                    TweetId = 4,
                    Notification = true,
                    CreationTime = DateTime.Parse("03.12.2021 12:12:12", culture),
                },
                new LikeModel
                {
                    Id = 5,
                    UserId = 1,
                    TweetId = 5,
                    Notification = true,
                    CreationTime = DateTime.Parse("04.12.2021 12:12:12", culture),
                },
                new LikeModel
                {
                    Id = 6,
                    UserId = 2,
                    TweetId = 1,
                    Notification = true,
                    CreationTime = DateTime.Parse("05.12.2021 12:12:12", culture),
                },
                new LikeModel
                {
                    Id = 7,
                    UserId = 2,
                    TweetId = 2,
                    Notification = true,
                    CreationTime = DateTime.Parse("04.12.2021 12:12:12", culture),
                },
                new LikeModel
                {
                    Id = 8,
                    UserId = 2,
                    TweetId = 3,
                    Notification = true,
                    CreationTime = DateTime.Parse("03.12.2021 12:12:12", culture),
                },
                new LikeModel
                {
                    Id = 9,
                    UserId = 2,
                    TweetId = 4,
                    Notification = true,
                    CreationTime = DateTime.Parse("02.03.2021 12:12:12", culture),
                },
                new LikeModel
                {
                    Id = 10,
                    UserId = 2,
                    TweetId = 5,
                    Notification = true,
                    CreationTime = DateTime.Parse("01.12.2021 12:12:12", culture),
                },
                new LikeModel
                {
                    Id = 11,
                    UserId = 3,
                    TweetId = 1,
                    Notification = true,
                    CreationTime = DateTime.Parse("02.12.2021 12:12:12", culture),
                },
                new LikeModel
                {
                    Id = 12,
                    UserId = 3,
                    TweetId = 2,
                    Notification = true,
                    CreationTime = DateTime.Parse("03.12.2021 12:12:12", culture),
                },
                new LikeModel
                {
                    Id = 13,
                    UserId = 3,
                    TweetId = 3,
                    Notification = true,
                    CreationTime = DateTime.Parse("04.12.2021 12:12:12", culture),
                },
                new LikeModel
                {
                    Id = 14,
                    UserId = 3,
                    TweetId = 4,
                    Notification = true,
                    CreationTime = DateTime.Parse("05.12.2021 12:12:12", culture),
                },
                new LikeModel
                {
                    Id = 15,
                    UserId = 3,
                    TweetId = 5,
                    Notification = true,
                    CreationTime = DateTime.Parse("07.12.2021 12:12:12", culture),
                },
                new LikeModel
                {
                    Id = 16,
                    UserId = 3,
                    TweetId = 7,
                    Notification = true,
                    CreationTime = DateTime.Parse("07.12.2021 12:12:12", culture),
                },
                new LikeModel
                {
                    Id = 17,
                    UserId = 6,
                    TweetId = 1,
                    Notification = true,
                    CreationTime = DateTime.Parse("07.12.2021 12:12:12", culture),
                },
                new LikeModel
                {
                    Id = 18,
                    UserId = 10,
                    TweetId = 8,
                    Notification = true,
                    CreationTime = DateTime.Parse("08.11.2021 12:12:12", culture),
                },
                new LikeModel
                {
                    Id = 19,
                    UserId = 9,
                    TweetId = 9,
                    Notification = true,
                    CreationTime = DateTime.Parse("09.11.2021 12:12:12", culture),
                },
                new LikeModel
                {
                    Id = 20,
                    UserId = 8,
                    TweetId = 10,
                    Notification = true,
                    CreationTime = DateTime.Parse("10.11.2021 12:12:12", culture),
                },
                new LikeModel
                {
                    Id = 21,
                    UserId = 6,
                    TweetId = 11,
                    Notification = true,
                    CreationTime = DateTime.Parse("11.11.2021 12:12:12", culture),
                },
                new LikeModel
                {
                    Id = 22,
                    UserId = 5,
                    TweetId = 12,
                    Notification = true,
                    CreationTime = DateTime.Parse("12.11.2021 12:12:12", culture),
                },
                new LikeModel
                {
                    Id = 23,
                    UserId = 4,
                    TweetId = 13,
                    Notification = true,
                    CreationTime = DateTime.Parse("13.10.2021 12:12:12", culture),
                },
                new LikeModel
                {
                    Id = 24,
                    UserId = 3,
                    TweetId = 13,
                    Notification = true,
                    CreationTime = DateTime.Parse("14.10.2021 12:12:12", culture),
                },
                new LikeModel
                {
                    Id = 25,
                    UserId = 2,
                    TweetId = 14,
                    Notification = true,
                    CreationTime = DateTime.Parse("15.10.2021 12:12:12", culture),
                },
                new LikeModel
                {
                    Id = 26,
                    UserId = 1,
                    TweetId = 14,
                    Notification = true,
                    CreationTime = DateTime.Parse("16.10.2021 12:12:12", culture),
                },
                new LikeModel
                {
                    Id = 27,
                    UserId = 10,
                    TweetId = 15,
                    Notification = true,
                    CreationTime = DateTime.Parse("17.10.2021 12:12:12", culture),
                },
                new LikeModel
                {
                    Id = 28,
                    UserId = 9,
                    TweetId = 15,
                    Notification = true,
                    CreationTime = DateTime.Parse("18.10.2021 12:12:12", culture),
                },
                new LikeModel
                {
                    Id = 29,
                    UserId = 8,
                    TweetId = 16,
                    Notification = true,
                    CreationTime = DateTime.Parse("19.10.2021 12:12:12", culture),
                },
                new LikeModel
                {
                    Id = 30,
                    UserId = 7,
                    TweetId = 16,
                    Notification = true,
                    CreationTime = DateTime.Parse("20.09.2021 12:12:12", culture),
                },
                new LikeModel
                {
                    Id = 31,
                    UserId = 3,
                    TweetId = 17,
                    Notification = true,
                    CreationTime = DateTime.Parse("21.10.2021 12:12:12", culture),
                },
                new LikeModel
                {
                    Id = 32,
                    UserId = 6,
                    TweetId = 17,
                    Notification = true,
                    CreationTime = DateTime.Parse("22.11.2021 12:12:12", culture),
                },
                new LikeModel
                {
                    Id = 33,
                    UserId = 5,
                    TweetId = 18,
                    Notification = true,
                    CreationTime = DateTime.Parse("23.12.2021 12:12:12", culture),
                },
                new LikeModel
                {
                    Id = 34,
                    UserId = 4,
                    TweetId = 19,
                    Notification = true,
                    CreationTime = DateTime.Parse("24.12.2021 12:12:12", culture),
                },
            };

            _base.Add(typeof(LikeModel), _likes);
        });

        private Task InitBookmarks() => Task.Run(() =>
        {
            var culture = CultureInfo.GetCultureInfo("ru-RU");

            _bookmarks = new List<Bookmark>
            {
                new Bookmark
                {
                    Id = 1,
                    UserId = 1,
                    TweetId = 1,
                    Notification = true,
                    CreationTime = DateTime.Parse("01.12.2021 12:12:12", culture),
                },
                new Bookmark
                {
                    Id = 2,
                    UserId = 1,
                    TweetId = 2,
                    Notification = true,
                    CreationTime = DateTime.Parse("10.12.2021 12:12:12", culture),
                },
                new Bookmark
                {
                    Id = 3,
                    UserId = 1,
                    TweetId = 3,
                    Notification = true,
                    CreationTime = DateTime.Parse("01.12.2021 12:12:12", culture),
                },
                new Bookmark
                {
                    Id = 4,
                    UserId = 1,
                    TweetId = 4,
                    Notification = true,
                    CreationTime = DateTime.Parse("05.12.2021 12:12:12", culture),
                },
                new Bookmark
                {
                    Id = 5,
                    UserId = 1,
                    TweetId = 5,
                    Notification = true,
                    CreationTime = DateTime.Parse("06.12.2021 12:12:12", culture),
                },
                new Bookmark
                {
                    Id = 6,
                    UserId = 2,
                    TweetId = 1,
                    Notification = true,
                    CreationTime = DateTime.Parse("07.12.2021 12:12:12", culture),
                },
                new Bookmark
                {
                    Id = 7,
                    UserId = 2,
                    TweetId = 2,
                    Notification = true,
                    CreationTime = DateTime.Parse("08.12.2021 12:12:12", culture),
                },
                new Bookmark
                {
                    Id = 8,
                    UserId = 2,
                    TweetId = 3,
                    Notification = true,
                    CreationTime = DateTime.Parse("09.12.2021 12:12:12", culture),
                },
                new Bookmark
                {
                    Id = 9,
                    UserId = 2,
                    TweetId = 4,
                    Notification = true,
                    CreationTime = DateTime.Parse("09.12.2021 12:12:12", culture),
                },
                new Bookmark
                {
                    Id = 10,
                    UserId = 2,
                    TweetId = 5,
                    Notification = true,
                    CreationTime = DateTime.Parse("08.12.2021 12:12:12", culture),
                },
                new Bookmark
                {
                    Id = 11,
                    UserId = 3,
                    TweetId = 1,
                    Notification = true,
                    CreationTime = DateTime.Parse("07.12.2021 12:12:12", culture),
                },
                new Bookmark
                {
                    Id = 12,
                    UserId = 3,
                    TweetId = 2,
                    Notification = true,
                    CreationTime = DateTime.Parse("06.12.2021 12:12:12", culture),
                },
                new Bookmark
                {
                    Id = 13,
                    UserId = 3,
                    TweetId = 3,
                    Notification = true,
                    CreationTime = DateTime.Parse("05.12.2021 12:12:12", culture),
                },
                new Bookmark
                {
                    Id = 14,
                    UserId = 3,
                    TweetId = 4,
                    Notification = true,
                    CreationTime = DateTime.Parse("04.12.2021 12:12:12", culture),
                },
                new Bookmark
                {
                    Id = 15,
                    UserId = 3,
                    TweetId = 5,
                    Notification = true,
                    CreationTime = DateTime.Parse("03.12.2021 12:12:12", culture),
                },
                new Bookmark
                {
                    Id = 16,
                    UserId = 6,
                    TweetId = 6,
                    Notification = true,
                    CreationTime = DateTime.Parse("01.11.2021 12:12:12", culture),
                },
                new Bookmark
                {
                    Id = 17,
                    UserId = 6,
                    TweetId = 7,
                    Notification = true,
                    CreationTime = DateTime.Parse("10.11.2021 12:12:12", culture),
                },
                new Bookmark
                {
                    Id = 18,
                    UserId = 7,
                    TweetId = 8,
                    Notification = true,
                    CreationTime = DateTime.Parse("01.11.2021 12:12:12", culture),
                },
                new Bookmark
                {
                    Id = 19,
                    UserId = 7,
                    TweetId = 9,
                    Notification = true,
                    CreationTime = DateTime.Parse("05.11.2021 12:12:12", culture),
                },
                new Bookmark
                {
                    Id = 20,
                    UserId = 8,
                    TweetId = 10,
                    Notification = true,
                    CreationTime = DateTime.Parse("06.11.2021 12:12:12", culture),
                },
                new Bookmark
                {
                    Id = 21,
                    UserId = 8,
                    TweetId = 11,
                    Notification = true,
                    CreationTime = DateTime.Parse("07.11.2021 12:12:12", culture),
                },
                new Bookmark
                {
                    Id = 22,
                    UserId = 9,
                    TweetId = 12,
                    Notification = true,
                    CreationTime = DateTime.Parse("08.10.2021 12:12:12", culture),
                },
                new Bookmark
                {
                    Id = 23,
                    UserId = 9,
                    TweetId = 13,
                    Notification = true,
                    CreationTime = DateTime.Parse("09.10.2021 12:12:12", culture),
                },
                new Bookmark
                {
                    Id = 24,
                    UserId = 10,
                    TweetId = 14,
                    Notification = true,
                    CreationTime = DateTime.Parse("09.10.2021 12:12:12", culture),
                },
                new Bookmark
                {
                    Id = 25,
                    UserId = 10,
                    TweetId = 15,
                    Notification = true,
                    CreationTime = DateTime.Parse("08.10.2021 12:12:12", culture),
                },
                new Bookmark
                {
                    Id = 26,
                    UserId = 1,
                    TweetId = 16,
                    Notification = true,
                    CreationTime = DateTime.Parse("07.10.2021 12:12:12", culture),
                },
                new Bookmark
                {
                    Id = 27,
                    UserId = 1,
                    TweetId = 17,
                    Notification = true,
                    CreationTime = DateTime.Parse("06.10.2021 12:12:12", culture),
                },
                new Bookmark
                {
                    Id = 28,
                    UserId = 2,
                    TweetId = 18,
                    Notification = true,
                    CreationTime = DateTime.Parse("05.10.2021 12:12:12", culture),
                },
                new Bookmark
                {
                    Id = 29,
                    UserId = 2,
                    TweetId = 19,
                    Notification = true,
                    CreationTime = DateTime.Parse("04.10.2021 12:12:12", culture),
                },
                new Bookmark
                {
                    Id = 30,
                    UserId = 3,
                    TweetId = 20,
                    Notification = true,
                    CreationTime = DateTime.Parse("03.10.2021 12:12:12", culture),
                },
            };

            _base.Add(typeof(Bookmark), _bookmarks);
        });

        #endregion
    }
}