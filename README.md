# Apache Kafka com .NET Core

Este simples projeto tem como objetivo mostrar a comunicação entre dois contêineres, produzindo e consumindo mensagens através do Apache Kafta.

Foram criados dois serviços que executam tarefas em background, uma para criar mensagens a cada 5 segundos e outra para ler as mensagens do tópico.

## Primeiro Passo - Rodando o Apache Kafka e realizar a criação de um tópico.

Primeiramente vamos clonar o repositório mantido pela Confluent.

```bash
git clone https://github.com/confluentinc/cp-docker-images -b 5.3.0-post
```

Depois navegamos para a pasta que contém o projeto do Apache Kafka.

```bash
cd cp-docker-images/examples/cp-all-in-one
```

Agora vamos rodar o Docker Compose.

```bash
docker-compose up -d --build
```

Pronto! Agora podemos acessar a dashboard do contêiner do Control Center para criarmos nosso primeiro tópico.

```
localhost:9021
```

## Segundo Passo - Rodando o aplicativo

Agora que o Apache Kafka está rodando, e criamos um tópico, vamos iniciar dois contêineres da nossa API.

```bash
cd PraticoBari
docker-compose up -d --build --scale app=2
```

Pronto! Agora podemos confirmar se nossos contêineres estão executando através do health check

```bash
localhost:<port>/health
```

Vamos abrir dois terminais e executar o código abaixo para acompanhar a comunicação de cada contêiner

```bash
 docker logs --follow <container id>
 ```

## Algumas informações

Podemos definir o nome do tópico no arquivo appsettings.json

```json
"msSettings": {
    "topicName": "my-topic"
}
```

Podemos pausar ou retomar a produção de mensagens através do endpoint

```
localhost:<port>/api/values/stop
localhost:<port>/api/values/start
```
