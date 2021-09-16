<h1>Sauron - Reconhecimento de pessoas em circuito de cameras IP</h1>

Está aplicação tem como objetivo identificar um indivíduo através do reconhecimento facial, que está cadastrado na base dados do sistema, em um circuito de câmeras de vigilância em ambiente privado e identificar a localização através da posição da câmera que está realizando a captura.

Foi desenvolvido para o projeto de conclusão da graduação de Ciência da Computação da universidade Ulbra, orientado pelo professor <b>Elgio Schlemer</b>.

<h2>Eu vejo tudo</h2>
<img src="https://user-images.githubusercontent.com/39543693/100042482-4b11ff80-2dea-11eb-8cc6-2f15dbcc3147.gif">

A ideia surgiu como uma maneira de auxiliar na prevenção a crimes posteriormente sendo visto que é possivel ser aplicado na localização pessoas desaparecidas. Uma tecnologia semelhante é utilizada em uma aplicação embarcada desenvolvida pela NTT(Nippon Telegraph and Telephone) e Earth Eyes Technology chamada de <a href="https://www.itmedia.co.jp/news/articles/1805/28/news085.html">AI Guardman</a>, porém com foco em prevenção a furto em comércios.

A aplicação funciona da seguinte forma, tendo imagens salvas em um diretório mapeado pela aplicação é realizado o treinamento da RNA disponibilizada pela Microsoft Azure. Utilizando câmeras IP para entrada das imagens em tempo real é realizada a captura do frame atual, convertido em uma imagem, verificado se é encontrado uma face e se caso encontrado a imagem é enviado a API de reconhecimento facial. Se na imagem enviada foi identificado a compatibilidade com alguma face no modelo treinado pela RNA, o response retorna o nome da pessoa, a acuracidade da comparação, as cordenadas para localização na imagem e alguns dados adicionais.

A face não cadastrada é contornada com um retângulo verde e a face cadastrada contornada com um retângulo vermelho e com informação do nome e acuracidade, exemplo abaixo.

![cadastradoenaocadastrado](https://user-images.githubusercontent.com/39543693/100171845-d1d7e280-2ea5-11eb-9cbe-051210d2f23d.png)


Mais detalhes sobre a aplicação e seu desenvolvimento basta dar uma olhada neste <a href="https://drive.google.com/file/d/1GPkyD_DP6m_AEGmEduzA79qdhBLJjYdr/view?usp=sharing">artigo</a>

<h4>Build</h4>
<blockquote>msbuild.exe c:\projetos\sauron\Sauron.sln\</blockquote>
